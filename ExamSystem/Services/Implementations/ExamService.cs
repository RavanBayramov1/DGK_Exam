using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class ExamService(IExamRepository _examRepository,
    IResultRepository _resultRepository,
    IQuestionRepository _questionRepository,
    IAnswerRepository _answerRepository) : IExamService
{
    public async Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync()
    {
        var exams = await _examRepository.GetAllAsync();
        return exams
            .Where(e => !e.IsDeleted)
            .Select(e => (ExamResponseDto)e)
            .ToList();
    }

    public async Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if(exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        return (ExamResponseDto)exam;
    }
    public async Task<ServiceResult> CreateAsync(CreateExamDto dto, int userrId)
    {
        Exam exam = dto;
        exam.UserId = userrId;
        await _examRepository.AddAsync(exam);
        return ServiceResult.Success();
    }
    public async Task<ServiceResult> UpdateAsync(int id, UpdateExamDto dto)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if(exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        if(exam.Status == "active")
            return Error.Validation("Aktiv imtahanı redaktə etmək olmaz!");

        exam.Title = dto.Title;
        exam.Duration = dto.Duration;
        exam.StartTime = dto.StartTime;
        exam.Status = dto.Status;
        await _examRepository.UpdateAsync(exam);
        return ServiceResult.Success();
    }

    //////////////////
    public async Task<ServiceResult<StartExamDto>> StartExamAsync(int examId, int studentId)
    {
        var exam = await _examRepository.GetByIdAsync(examId);
        if (exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı.");

        if (exam.Status != "active")
            return Error.Validation("İmtahan aktiv deyil.");

        var existing = await _resultRepository.GetByStudentIdAsync(studentId);
        if (existing.Any(r => r.ExamId == examId && !r.IsDeleted))
            return Error.Conflict("Bu tələbə artıq bu imtahana girib.");

        var result = new Models.Result
        {
            ExamId = examId,
            StudentId = studentId,
            Score = 0,
            Status = "pending",
            TakenAt = DateTime.UtcNow
        };
        await _resultRepository.AddAsync(result);

        var questions = await _questionRepository.GetByExamIdAsync(examId);

        return new StartExamDto
        {
            ResultId = result.Id,
            ExamTitle = exam.Title,
            Duration = exam.Duration,
            Questions = questions
                .Where(q => !q.IsDeleted)
                .Select(q => (QuestionResponseDto)q)
                .ToList()
        };
    }

    public async Task<ServiceResult> SubmitExamAsync(SubmitExamDto dto)
    {
        var result = await _resultRepository.GetByIdAsync(dto.ResultId);
        if (result == null || result.IsDeleted)
            return Error.NotFound("Nəticə tapılmadı.");

        var questions = await _questionRepository.GetByExamIdAsync(result.ExamId);
        var questionDict = questions.ToDictionary(q => q.Id);

        int totalScore = 0;
        foreach (var answerDto in dto.Answers)
        {
            if (!questionDict.TryGetValue(answerDto.QuestionId, out var question))
                continue;

            var isCorrect = question.CorrectAnswer == answerDto.AnswerText;
            var earnedPoint = isCorrect ? question.Point : 0;
            totalScore += earnedPoint;

            var answer = new Models.Answer
            {
                ResultId = dto.ResultId,
                QuestionId = answerDto.QuestionId,
                AnswerText = answerDto.AnswerText,
                IsCorrect = isCorrect,
                EarnedPoint = earnedPoint
            };
            await _answerRepository.AddAsync(answer);
        }

        result.Score = totalScore;
        result.Status = "graded";
        await _resultRepository.UpdateAsync(result);
        return ServiceResult.Success();
    }
    ////////////////
    
    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if (exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        if(exam.Status == "active")
            return Error.Validation("Aktiv imtahanı silmək olmaz!");
        await _examRepository.DeleteAsync(id);
        return ServiceResult.Success();
    }

}
