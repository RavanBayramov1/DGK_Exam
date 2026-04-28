using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ResultDtos;

public class UpdateResultDto
{
    [Required(ErrorMessage = "Status boş ola bilməz.")]
    [RegularExpression("pending|graded", ErrorMessage = "Status pending və ya graded olmalıdır.")]
    public string Status { get; set; } = string.Empty;
}
