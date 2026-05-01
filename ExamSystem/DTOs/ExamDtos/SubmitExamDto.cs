using ExamSystem.DTOs.AnswerDtos;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ExamDtos;

public class SubmitExamDto
{
    [Required(ErrorMessage = "Nəticə Id boş ola bilməz.")]
    public int ResultId { get; set; }

    [Required(ErrorMessage = "Cavablar boş ola bilməz.")]
    [MinLength(1, ErrorMessage = "Ən azı bir cavab olmalıdır.")]
    public List<CreateAnswerDto> Answers { get; set; } = new();
}
