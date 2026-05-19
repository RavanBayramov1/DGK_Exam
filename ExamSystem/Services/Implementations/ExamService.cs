using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class ExamService(IExamRepository _examRepo,IResultRepository _resultRepo, IExamQuestionRepository _examQuestionRepo) : IExamService
{
    public async Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync()
    {
        var exams = await _examRepo.GetAllAsync();
        var result = exams.Select(e => (ExamResponseDto)e).ToList();
        return ServiceResult<List<ExamResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id)
    {
        var exam = await _examRepo.GetWithDetailsAsync(id);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        return ServiceResult<ExamResponseDto>.Success(exam);
    }

    public async Task<ServiceResult> CreateAsync(CreateExamDto dto, int teacherId)
    {
        Exam exam = dto;
        exam.TeacherId = teacherId;

        await _examRepo.AddAsync(exam);
        await _examRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateExamDto dto, int teacherId)
    {
        var exam = await _examRepo.GetByIdAsync(id);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.TeacherId != teacherId)
            return Error.Unauthorized("Bu imtahanı dəyişməyə icazəniz yoxdur.");

        if (exam.Status != ExamStatus.Draft)
            return Error.Validation("Yalnız Draft statusunda olan imtahan dəyişdirilə bilər.");

        exam.Title = dto.Title;
        exam.StartTime = dto.StartTime;
        exam.DurationMinutes = dto.DurationMinutes;
        exam.ShuffleQuestions = dto.ShuffleQuestions;
        exam.ShuffleOptions = dto.ShuffleOptions;
        exam.ShowResultsToStudent = dto.ShowResultsToStudent;
        exam.GroupId = dto.GroupId;
        exam.SubjectId = dto.SubjectId;

        _examRepo.Update(exam);
        await _examRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id, int teacherId)
    {
        var exam = await _examRepo.GetByIdAsync(id);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.TeacherId != teacherId)
            return Error.Unauthorized("Bu imtahanı silməyə icazəniz yoxdur.");

        _examRepo.SoftDelete(exam);
        await _examRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<StartExamDto>> StartExamAsync(int examId, int studentId)
    {
        var exam = await _examRepo.GetWithDetailsAsync(examId);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.Status != ExamStatus.Active)
            return Error.Validation("İmtahan aktiv deyil.");

        var existingResult = await _resultRepo.GetByExamAndStudentAsync(examId, studentId);
        if (existingResult is not null)
            return Error.Conflict("Siz bu imtahana artıq başlamısınız.");

        var result = new ExamResult
        {
            ExamId = examId,
            StudentId = studentId,
            StartedAt = DateTime.UtcNow
        };

        await _resultRepo.AddAsync(result);
        await _resultRepo.SaveChangesAsync();

        var questions = exam.ExamQuestions
            .OrderBy(eq => exam.ShuffleQuestions ? Guid.NewGuid() : (object)eq.OrderIndex)
            .Select(eq => (QuestionResponseDto)eq.Question)
            .ToList();

        var startExamDto = new StartExamDto
        {
            ResultId = result.Id,
            ExamTitle = exam.Title,
            Duration = exam.DurationMinutes,
            Questions = questions
        };

        return ServiceResult<StartExamDto>.Success(startExamDto);
    }

    public async Task<ServiceResult> SubmitExamAsync(SubmitExamDto dto)
    {
        var result = await _resultRepo.GetByIdAsync(dto.ResultId);
        if (result is null)
            return Error.NotFound("Nəticə tapılmadı.");

        if (result.SubmittedAt is not null)
            return Error.Conflict("İmtahan artıq təhvil verilib.");

        var exam = await _examRepo.GetWithDetailsAsync(result.ExamId);

        var givenAnswers = new List<StudentAnswerData>();
        decimal totalScore = 0;

        foreach (var answer in dto.Answers)
        {
            var examQuestion = exam.ExamQuestions
                .FirstOrDefault(eq => eq.QuestionId == answer.QuestionId);

            if (examQuestion is null) continue;

            var question = examQuestion.Question;
            var isCorrect = question.Type != QuestionType.OpenText &&
                answer.SelectedOptions.OrderBy(x => x)
                .SequenceEqual(question.CorrectAnswers.OrderBy(x => x));

            var pointsEarned = isCorrect ? examQuestion.Points : 0;
            totalScore += pointsEarned;

            givenAnswers.Add(new StudentAnswerData
            {
                QuestionId = answer.QuestionId,
                SelectedOptions = answer.SelectedOptions,
                CorrectOptions = question.CorrectAnswers,
                IsCorrect = question.Type == QuestionType.OpenText ? null : isCorrect,
                PointsEarned = pointsEarned
            });
        }

        result.GivenAnswers = givenAnswers;
        result.OriginalScore = totalScore;
        result.FinalScore = totalScore;
        result.SubmittedAt = DateTime.UtcNow;
        result.IsAutoSubmitted = false;

        _resultRepo.Update(result);
        await _resultRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
    public async Task<ServiceResult> AddQuestionToExamAsync(int examId, int questionId, decimal points, int teacherId)
    {
        var exam = await _examRepo.GetByIdAsync(examId);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.TeacherId != teacherId)
            return Error.Unauthorized("Bu imtahana sual əlavə etməyə icazəniz yoxdur.");

        if (exam.Status != ExamStatus.Draft)
            return Error.Validation("Yalnız Draft statusunda olan imtahana sual əlavə edilə bilər.");

        var existing = await _examQuestionRepo.GetByExamIdAsync(examId);
        if (existing.Any(eq => eq.QuestionId == questionId))
            return Error.Conflict("Bu sual artıq imtahana əlavə edilib.");

        var examQuestion = new ExamQuestion
        {
            ExamId = examId,
            QuestionId = questionId,
            Points = points,
            OrderIndex = existing.Count + 1
        };

        await _examQuestionRepo.AddAsync(examQuestion);
        await _examQuestionRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RemoveQuestionFromExamAsync(int examId, int questionId, int teacherId)
    {
        var exam = await _examRepo.GetByIdAsync(examId);
        if (exam is null)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.TeacherId != teacherId)
            return Error.Unauthorized("Bu imtahandan sual silməyə icazəniz yoxdur.");

        if (exam.Status != ExamStatus.Draft)
            return Error.Validation("Yalnız Draft statusunda olan imtahandan sual silinə bilər.");

        var existing = await _examQuestionRepo.GetByExamIdAsync(examId);
        var examQuestion = existing.FirstOrDefault(eq => eq.QuestionId == questionId);
        if (examQuestion is null)
            return Error.NotFound("Bu sual imtahanda mövcud deyil.");

        _examQuestionRepo.SoftDelete(examQuestion);
        await _examQuestionRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
