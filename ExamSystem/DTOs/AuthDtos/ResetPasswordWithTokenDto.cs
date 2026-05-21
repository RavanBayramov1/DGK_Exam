using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class ResetPasswordWithTokenDto
{
    [Required(ErrorMessage = "Email mütləq olmalıdır.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Token mütləq olmalıdır.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifrə mütləq daxil edilməlidir.")]
    [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvol olmalıdır.")]
    public string NewPassword { get; set; } = string.Empty;
}
