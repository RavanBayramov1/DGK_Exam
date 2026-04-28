using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Ad və Soyad boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Ad və Soyad minimum 8 simvol olmalıdır.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvol olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol boş ola bilməz.")]
    [RegularExpression("Admin|Teacher|Student", ErrorMessage = "Rol Admin, Teacher və ya Student olmalıdır.")]
    public string Role { get; set; } = string.Empty;
}
