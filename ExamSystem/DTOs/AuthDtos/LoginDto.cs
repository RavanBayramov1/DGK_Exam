using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class LoginDto
{
    [EmailAddress(ErrorMessage = "Email formatı yanlışdır.")]
    public string Email { get; set; } = string.Empty;  


    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    public string Password { get; set; } = string.Empty;
}
