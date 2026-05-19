using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Models;

namespace ExamSystem.DTOs.GroupDtos;

public class GroupDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<UserSummaryDto> Students { get; set; }
    public List<UserSummaryDto> Teachers { get; set; }

    public static implicit operator GroupDetailDto(Group group) => new()
    {
        Id = group.Id,
        Name = group.Name,
        Students = group.Students?.Select(s => new UserSummaryDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Role = s.Role.ToString()
        }).ToList(),
        Teachers = group.Teachers?.Select(t => new UserSummaryDto
        {
            Id = t.Id,
            FullName = t.FullName,
            Email = t.Email,
            Role = t.Role.ToString()
        }).ToList()
    };
}
