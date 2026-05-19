using ExamSystem.Models;

namespace ExamSystem.DTOs.SubjectDtos;

public class SubjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static implicit operator SubjectResponseDto(Subject subject) => new()
    {
        Id = subject.Id,
        Name = subject.Name
    };
}
