using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class LoginDto
{
    [Required(ErrorMessage = "Ad boş ola bilməz.")]
    public string FullName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    public string Password { get; set; } = string.Empty;
}
