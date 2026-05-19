using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Ad və Soyad boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Ad və Soyad minimum 8 simvol olmalıdır.")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email formatı yanlışdır.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
    [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvol olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    public static implicit operator AppUser(RegisterDto dto) => new()
    {
        FullName = dto.FullName,
        Email = dto.Email
    };
}

