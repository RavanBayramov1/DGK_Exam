using ExamSystem.Models;

namespace ExamSystem.DTOs.QuestionDtos;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Options { get; set; } = string.Empty;
    //public string CorrectAnswer { get; set; } = string.Empty;
    public int Point { get; set; }


    public static implicit operator QuestionResponseDto(Question question) => new()
    {
        Id = question.Id,
        ExamId = question.ExamId,
        Text = question.Text,
        Type = question.Type,
        Options = question.Options,
        Point = question.Point
    };
}
