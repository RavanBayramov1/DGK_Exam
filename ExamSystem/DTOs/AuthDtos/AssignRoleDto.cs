using ExamSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.AuthDtos;

public class AssignRoleDto
{
    [EnumDataType(typeof(UserRole), ErrorMessage = "Rol düzgün deyil.")]
    public UserRole Role { get; set; }      
    public int? GroupId { get; set; }  // Yalnız Student üçün
}