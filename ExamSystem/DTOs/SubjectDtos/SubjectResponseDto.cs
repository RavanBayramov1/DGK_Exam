using ExamSystem.Models;

namespace ExamSystem.DTOs.SubjectDtos;

public class SubjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static implicit operator SubjectResponseDto(Subject subject)
    {

        if (subject == null) return null;

        return new SubjectResponseDto
        {
            Id = subject.Id,
            Name = subject.Name 
        };
    }
}
