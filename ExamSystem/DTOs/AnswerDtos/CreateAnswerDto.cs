using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AnswerDtos;

public class CreateAnswerDto
{
    [Required(ErrorMessage = "Nəticə boş ola bilməz.")]
    public int ResultId { get; set; }

    [Required(ErrorMessage = "Sual boş ola bilməz.")]
    public int QuestionId { get; set; }

    [Required(ErrorMessage = "Cavab mətni boş ola bilməz.")]
    public string AnswerText { get; set; } = string.Empty;


    public static implicit operator Answer(CreateAnswerDto dto) => new()
    {
        ResultId = dto.ResultId,
        QuestionId = dto.QuestionId,
        AnswerText = dto.AnswerText,
        IsCorrect = false,
        EarnedPoint = 0
    };
}
