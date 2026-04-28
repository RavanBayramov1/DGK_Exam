using ExamSystem.Models;

namespace ExamSystem.DTOs.AnswerDtos;

public class AnswerResponseDto
{
    public int Id { get; set; }
    public int ResultId { get; set; }
    public int QuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int EarnedPoint { get; set; }


    public static implicit operator AnswerResponseDto(Answer answer) => new()
    {
        Id = answer.Id,
        ResultId = answer.ResultId,
        QuestionId = answer.QuestionId,
        AnswerText = answer.AnswerText,
        IsCorrect = answer.IsCorrect,
        EarnedPoint = answer.EarnedPoint
    };
}
