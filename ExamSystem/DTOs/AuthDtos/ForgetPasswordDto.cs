using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class ForgetPasswordDto
{
    [Required(ErrorMessage = "Email mütləq daxil edilməlidir.")]
    [EmailAddress(ErrorMessage = "Düzgün bir email ünvanı daxil edin.")]
    public string Email { get; set; } = string.Empty;
}
