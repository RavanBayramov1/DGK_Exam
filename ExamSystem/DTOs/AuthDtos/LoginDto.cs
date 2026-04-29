using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class LoginDto
{
    [Required(ErrorMessage = "İstifadəçi adı boş ola bilməz.")]
    [MinLength(4, ErrorMessage = "İstifadəçi adı minimum 4 simvol olmalıdır.")]
    public string UserName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    public string Password { get; set; } = string.Empty;
}
