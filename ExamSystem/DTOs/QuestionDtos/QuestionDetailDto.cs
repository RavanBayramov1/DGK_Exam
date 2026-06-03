using ExamSystem.DTOs.SubjectDtos;
using ExamSystem.Enums;
using ExamSystem.Models;

namespace ExamSystem.DTOs.QuestionDtos;

public class QuestionDetailDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public decimal DefaultPoints { get; set; }
    public List<string> Options { get; set; }
    public List<string> CorrectAnswers { get; set; }  // ← burda var
    public SubjectResponseDto Subject { get; set; }
    public string? MediaUrl { get; set; }

    public static implicit operator QuestionDetailDto(Question question)
    {
        if (question == null) return null;
        return new QuestionDetailDto
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            Type = question.Type,
            DefaultPoints = question.DefaultPoints,
            Options = question.Options,
            CorrectAnswers = question.CorrectAnswers,
            Subject = question.Subject != null ? (SubjectResponseDto)question.Subject : null,
            MediaUrl = null
        };
    }
}
