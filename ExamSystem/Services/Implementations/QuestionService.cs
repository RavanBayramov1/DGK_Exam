using ExamSystem.Common;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Models;
using ExamSystem.Services.Interfaces;
using ExamSystem.Repositories.Interfaces;

namespace ExamSystem.Services.Implementations;

public class QuestionService(IQuestionRepository _questionRepository) : IQuestionService
{

    public async Task<ServiceResult<List<QuestionResponseDto>>> GetAllAsync()
    {
        var questions = await _questionRepository.GetAllAsync();
        return questions
            .Where(q => !q.IsDeleted)
            .Select(q => (QuestionResponseDto)q)
            .ToList();
    }

    public async Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId)
    {
        var question = await _questionRepository.GetByExamIdAsync(examId);
        if (!question.Any())
            return Error.NotFound("İmtahan sualları tapılmadı!");
        return question.Select(q => (QuestionResponseDto)q).ToList();

    }

    public async Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if(question == null || question.IsDeleted)
            return Error.NotFound("Suallar tapılmadı!");
        return (QuestionResponseDto)question;
    }
    public async Task<ServiceResult> CreateAsync(CreateQuestionDto dto)
    {
        Question question = dto;
        await _questionRepository.AddAsync(question);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateQuestionDto dto)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if (question == null || question.IsDeleted)
            return Error.NotFound("Sual tapılmadı!");

        question.Text = dto.Text;
        question.Type = dto.Type;
        question.Options = dto.Options;
        question.CorrectAnswer = dto.CorrectAnswer;
        question.Point = dto.Point;

        await _questionRepository.UpdateAsync(question);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if(question == null || question.IsDeleted)
            return Error.NotFound("Belə bir sual yoxdur!");
        await _questionRepository.DeleteAsync(id);
        return ServiceResult.Success();
    }
}
