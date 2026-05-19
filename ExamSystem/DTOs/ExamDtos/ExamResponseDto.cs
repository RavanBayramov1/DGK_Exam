using ExamSystem.DTOs.AuthDtos;
using ExamSystem.DTOs.GroupDtos;
using ExamSystem.DTOs.SubjectDtos;
using ExamSystem.Enums;
using ExamSystem.Models;

namespace ExamSystem.DTOs.ExamDtos;

public class ExamResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public ExamStatus Status { get; set; }
    public bool ShuffleQuestions { get; set; }
    public bool ShuffleOptions { get; set; }
    public bool ShowResultsToStudent { get; set; }
    public GroupResponseDto Group { get; set; }
    public SubjectResponseDto Subject { get; set; }
    public UserSummaryDto Teacher { get; set; }

    public static implicit operator ExamResponseDto(Exam exam) => new()
    {
        Id = exam.Id,
        Title = exam.Title,
        StartTime = exam.StartTime,
        DurationMinutes = exam.DurationMinutes,
        Status = exam.Status,
        ShuffleQuestions = exam.ShuffleQuestions,
        ShuffleOptions = exam.ShuffleOptions,
        ShowResultsToStudent = exam.ShowResultsToStudent,
        Group = exam.Group,
        Subject = exam.Subject,
        Teacher = exam.Teacher is null ? null : new UserSummaryDto
        {
            Id = exam.Teacher.Id,
            FullName = exam.Teacher.FullName,
            Email = exam.Teacher.Email,
            Role = exam.Teacher.Role.ToString()
        }
    };
}
