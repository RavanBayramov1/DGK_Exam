using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class LoginDto
{
    [Required(ErrorMessage = "Email boş ola bilməz.")]
    [EmailAddress(ErrorMessage = "Email formatı yanlışdır.")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    public string Password { get; set; } = string.Empty;
}
