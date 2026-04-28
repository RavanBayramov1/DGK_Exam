using ExamSystem.Models;

namespace ExamSystem.DTOs.ExamDtos;

public class ExamResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateTime StartTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public int UserId { get; set; }


    public static implicit operator ExamResponseDto(Exam exam) => new()
    {
        Id = exam.Id,
        Title = exam.Title,
        Duration = exam.Duration,
        StartTime = exam.StartTime,
        Status = exam.Status,
        UserId = exam.UserId
    };
}
