using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.SubjectDtos;

public class CreateSubjectDto
{
    [Required(ErrorMessage = "Fənn adı boş ola bilməz.")]
    [MinLength(2, ErrorMessage = "Fənn adı minimum 2 simvol olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    public static implicit operator Subject(CreateSubjectDto dto) => new()
    {
        Name = dto.Name
    };
}
