using ExamSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExamSystem.DTOs.AuthDtos;

public class AssignRoleDto
{
    [EnumDataType(typeof(UserRole), ErrorMessage = "Rol düzgün deyil.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}