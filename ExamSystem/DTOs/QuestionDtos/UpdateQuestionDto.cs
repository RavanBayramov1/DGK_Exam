using ExamSystem.Enums;
using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.QuestionDtos;

public class UpdateQuestionDto
{
    [Required(ErrorMessage = "Sual mətni boş ola bilməz.")]
    [MinLength(5, ErrorMessage = "Sual mətni minimum 5 simvol olmalıdır.")]
    public string QuestionText { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tip boş ola bilməz.")]
    public QuestionType Type { get; set; }

    [Range(0.1, 100, ErrorMessage = "Bal 0.1 ilə 100 arasında olmalıdır.")]
    public decimal DefaultPoints { get; set; }

    public List<string> Options { get; set; }
    public List<string> CorrectAnswers { get; set; }

    [Required(ErrorMessage = "Fənn boş ola bilməz.")]
    public int SubjectId { get; set; }
}
