using ExamSystem.Models;

namespace ExamSystem.DTOs.GroupDtos;

public class GroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public int TeacherCount { get; set; }

    public static implicit operator GroupResponseDto(Group group) => new()
    {
        Id = group.Id,
        Name = group.Name,
        StudentCount = group.Students?.Count ?? 0,
        TeacherCount = group.Teachers?.Count ?? 0
    };
}
