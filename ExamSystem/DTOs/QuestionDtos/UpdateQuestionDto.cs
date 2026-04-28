using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.QuestionDtos;

public class UpdateQuestionDto
{
    [Required(ErrorMessage = "Sual mətni boş ola bilməz.")]
    [MinLength(5, ErrorMessage = "Sual mətni minimum 5 simvol olmalıdır.")]
    public string Text { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tip boş ola bilməz.")]
    [RegularExpression("multiple_choice|true_false|open", ErrorMessage = "Tip multiple_choice, true_false və ya open olmalıdır.")]
    public string Type { get; set; } = string.Empty;

    public string Options { get; set; } = string.Empty;

    [Required(ErrorMessage = "Düzgün cavab boş ola bilməz.")]
    public string CorrectAnswer { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "Bal 1-100 arasında olmalıdır.")]
    public int Point { get; set; }


    public static implicit operator Question(UpdateQuestionDto dto) => new()
    {
        Text = dto.Text,
        Type = dto.Type,
        Options = dto.Options,
        CorrectAnswer = dto.CorrectAnswer,
        Point = dto.Point
    };
}
