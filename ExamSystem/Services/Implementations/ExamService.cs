using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class ExamService(IExamRepository _examRepo,IResultRepository _resultRepo, IExamQuestionRepository _examQuestionRepo) :IExamService
{
    public async Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync()
    {
        var exams = await _examRepo.GetAllWithDetailsAsync();
        var result = exams.Select(e => (ExamResponseDto)e).ToList();
        return ServiceResult<List<ExamResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id)
    {
        var exam = await _examRepo.GetWithDetailsAsync(id);
        if (exam is null)
            return ErrorMessages.Exam.NotFound;

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
            return ErrorMessages.Exam.NotFound;

        if (exam.TeacherId != teacherId)
            return ErrorMessages.Exam.Unauthorized;

        if (exam.Status != ExamStatus.Draft)
            return ErrorMessages.Exam.NotDraft;

        exam.Title = dto.Title;
        exam.StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc);
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
            return ErrorMessages.Exam.NotFound;

        if (exam.TeacherId != teacherId)
            return ErrorMessages.Exam.Unauthorized;

        _examRepo.SoftDelete(exam);
        await _examRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<StartExamDto>> StartExamAsync(int examId, int studentId)
    {
        var exam = await _examRepo.GetWithDetailsAsync(examId);
        if (exam is null)
            return ErrorMessages.Exam.NotFound;

        if (exam.Status != ExamStatus.Active)
            return ErrorMessages.Exam.NotActive;

        var existingResult = await _resultRepo.GetByExamAndStudentAsync(examId, studentId);
        if (existingResult is not null)
            return ErrorMessages.Exam.AlreadyStarted;

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
            DurationMinutes = exam.DurationMinutes,
            Questions = questions
        };

        return ServiceResult<StartExamDto>.Success(startExamDto);
    }

    public async Task<ServiceResult> SubmitExamAsync(SubmitExamDto dto, int studentId)
    {
        var result = await _resultRepo.GetByIdAsync(dto.ResultId);
        if (result is null)
            return ErrorMessages.Exam.NotFound;

        if (result.StudentId != studentId)
            return ErrorMessages.Exam.Unauthorized;

        if (result.SubmittedAt is not null)
            return ErrorMessages.Exam.AlreadySubmitted;

        var exam = await _examRepo.GetWithDetailsAsync(result.ExamId);
        if (exam is null)
            return ErrorMessages.Exam.NotFound;

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
        result.IsAutoSubmitted = dto.Answers.Count == 0;

        _resultRepo.Update(result);
        await _resultRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
    public async Task<ServiceResult> AutoSubmitExamAsync(int resultId)
    {
        var result = await _resultRepo.GetByIdAsync(resultId);
        if (result is null)
            return ErrorMessages.Exam.NotFound;

        if (result.SubmittedAt is not null)
            return ServiceResult.Success();

        result.GivenAnswers = new List<StudentAnswerData>();
        result.OriginalScore = 0;
        result.FinalScore = 0;
        result.SubmittedAt = DateTime.UtcNow;
        result.IsAutoSubmitted = true;

        _resultRepo.Update(result);
        await _resultRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> AddQuestionToExamAsync(int examId, int questionId, decimal points, int teacherId)
    {
        var exam = await _examRepo.GetByIdAsync(examId);
        if (exam is null)
            return ErrorMessages.Exam.NotFound;

        if (exam.TeacherId != teacherId)
            return ErrorMessages.Exam.Unauthorized;

        if (exam.Status != ExamStatus.Draft)
            return ErrorMessages.Exam.NotDraft;

        var existing = await _examQuestionRepo.GetByExamIdAsync(examId);
        if (existing.Any(eq => eq.QuestionId == questionId))
            return ErrorMessages.Exam.QuestionAlreadyExists;

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
            return ErrorMessages.Exam.NotFound;

        if (exam.TeacherId != teacherId)
            return ErrorMessages.Exam.Unauthorized;

        if (exam.Status != ExamStatus.Draft)
            return ErrorMessages.Exam.NotDraft;

        var existing = await _examQuestionRepo.GetByExamIdAsync(examId);
        var examQuestion = existing.FirstOrDefault(eq => eq.QuestionId == questionId);
        if (examQuestion is null)
            return ErrorMessages.Exam.QuestionNotFound;

        _examQuestionRepo.SoftDelete(examQuestion);
        await _examQuestionRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
