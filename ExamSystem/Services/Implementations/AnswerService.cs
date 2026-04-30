using ExamSystem.Common;
using ExamSystem.DTOs.AnswerDtos;
using ExamSystem.Models;
using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class AnswerService(IAnswerRepository _answerRepository, IQuestionRepository _questionRepository) : IAnswerService
{
    public async Task<ServiceResult<List<AnswerResponseDto>>> GetByResultIdAsync(int resultId)
    {
        var answers = await _answerRepository.GetByResultIdAsync(resultId);
        return answers
            .Where(a => !a.IsDeleted)
            .Select(a => (AnswerResponseDto)a)
            .ToList();
    }
    public async Task<ServiceResult> CreateAsync(CreateAnswerDto dto)
    {
        var question = await _questionRepository.GetByIdAsync(dto.QuestionId);
        if (question == null || question.IsDeleted)
            return Error.NotFound($"{dto.QuestionId} id-li sual tapılmadı.");

        var isCorrected = question.CorrectAnswer == dto.AnswerText;
        var earnePoint = isCorrected ? question.Point : 0;
        var answer = new Answer
        {
            ResultId = dto.ResultId,
            QuestionId = dto.QuestionId,
            AnswerText = dto.AnswerText,
            IsCorrect =  dto.IsCorrect,
            EarnedPoint = dto.EarnedPoint
        };
        await _answerRepository.AddAsync(answer);
        return ServiceResult.Success();
    }
}
