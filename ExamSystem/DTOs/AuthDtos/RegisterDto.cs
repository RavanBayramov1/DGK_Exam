using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Ad və Soyad boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Ad və Soyad minimum 8 simvol olmalıdır.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "İstifadəçi adı boş ola bilməz.")]
    [MinLength(4, ErrorMessage = "İstifadəçi adı minimum 4 simvol olmalıdır.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvol olmalıdır.")]
    public string Password { get; set; } = string.Empty;
}
