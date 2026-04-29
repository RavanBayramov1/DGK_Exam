using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class ResetPasswordDto
{
    [Required]
    [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvol olmalıdır.")]
    public string NewPassword { get; set; } = string.Empty;
}
