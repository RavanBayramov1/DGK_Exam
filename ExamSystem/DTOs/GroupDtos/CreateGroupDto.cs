using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.GroupDtos;

public class CreateGroupDto
{
    [Required(ErrorMessage = "Qrup adı boş ola bilməz.")]
    [MinLength(2, ErrorMessage = "Qrup adı minimum 2 simvol olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    public static implicit operator Group(CreateGroupDto dto) => new()
    {
        Name = dto.Name
    };
}
