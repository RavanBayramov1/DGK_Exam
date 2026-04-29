using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class AssignRoleDto
{
    [Required]
    [RegularExpression("Admin|Teacher|Student", ErrorMessage = "Rol Admin, Teacher və ya Student olmalıdır.")]
    public string Role { get; set; } = string.Empty;
}