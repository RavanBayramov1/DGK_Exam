using ExamSystem.Common;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Models;
using ExamSystem.Services.Interfaces;
using ExamSystem.Repositories.Interfaces;

namespace ExamSystem.Services.Implementations;

public class QuestionService(IQuestionRepository _questionRepo) : IQuestionService
{
    public async Task<ServiceResult<List<QuestionResponseDto>>> GetAllAsync()
    {
        var questions = await _questionRepo.GetAllAsync();
        var result = questions.Select(q => (QuestionResponseDto)q).ToList();
        return ServiceResult<List<QuestionResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id)
    {
        var question = await _questionRepo.GetByIdAsync(id);
        if (question is null)
            return Error.NotFound("Sual tapılmadı.");

        return ServiceResult<QuestionResponseDto>.Success(question);
    }

    public async Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId)
    {
        var questions = await _questionRepo.GetByExamIdAsync(examId);
        var result = questions.Select(q => (QuestionResponseDto)q).ToList();
        return ServiceResult<List<QuestionResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<QuestionResponseDto>> CreateAsync(CreateQuestionDto dto, int teacherId)
    {
        Question question = dto;
        question.TeacherId = teacherId;

        await _questionRepo.AddAsync(question);
        await _questionRepo.SaveChangesAsync();

        return ServiceResult<QuestionResponseDto>.Success(question);
    }

    public async Task<ServiceResult<QuestionResponseDto>> UpdateAsync(int id, UpdateQuestionDto dto, int teacherId)
    {
        var question = await _questionRepo.GetByIdAsync(id);
        if (question is null)
            return Error.NotFound("Sual tapılmadı.");

        if (question.TeacherId != teacherId)
            return Error.Unauthorized("Bu sualı dəyişməyə icazəniz yoxdur.");

        question.QuestionText = dto.QuestionText;
        question.Type = dto.Type;
        question.DefaultPoints = dto.DefaultPoints;
        question.Options = dto.Options;
        question.CorrectAnswers = dto.CorrectAnswers;
        question.SubjectId = dto.SubjectId;

        _questionRepo.Update(question);
        await _questionRepo.SaveChangesAsync();

        return ServiceResult<QuestionResponseDto>.Success(question);
    }

    public async Task<ServiceResult> DeleteAsync(int id, int teacherId)
    {
        var question = await _questionRepo.GetByIdAsync(id);
        if (question is null)
            return Error.NotFound("Sual tapılmadı.");

        if (question.TeacherId != teacherId)
            return Error.Unauthorized("Bu sualı silməyə icazəniz yoxdur.");

        _questionRepo.SoftDelete(question);
        await _questionRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}