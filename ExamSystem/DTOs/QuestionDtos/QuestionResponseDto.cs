using ExamSystem.DTOs.SubjectDtos;
using ExamSystem.Enums;
using ExamSystem.Models;

namespace ExamSystem.DTOs.QuestionDtos;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public decimal DefaultPoints { get; set; }
    public List<string> Options { get; set; }
    public SubjectResponseDto Subject { get; set; }

    public static implicit operator QuestionResponseDto(Question question)
    {
        if (question == null) return null;

        return new QuestionResponseDto
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            Type = question.Type,
            DefaultPoints = question.DefaultPoints,
            Options = question.Options,
            Subject = question.Subject != null ? (SubjectResponseDto)question.Subject : null

        };
    }
}