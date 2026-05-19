using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Models;

namespace ExamSystem.DTOs.ResultDtos;

public class ResultResponseDto
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public UserSummaryDto Student { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsAutoSubmitted { get; set; }
    public decimal OriginalScore { get; set; }
    public decimal FinalScore { get; set; }
    public bool IsEditedByTeacher { get; set; }
    // public List<StudentAnswerData> GivenAnswers { get; set; } // Student öz cavablarını görəndə

    public static implicit operator ResultResponseDto(ExamResult result) => new()
    {
        Id = result.Id,
        ExamId = result.ExamId,
        ExamTitle = result.Exam.Title,
        Student = new UserSummaryDto
        {
            Id = result.Student.Id,
            FullName = result.Student.FullName,
            Email = result.Student.Email,
            Role = result.Student.Role.ToString()
        },
        StartedAt = result.StartedAt,
        SubmittedAt = result.SubmittedAt,
        IsAutoSubmitted = result.IsAutoSubmitted,
        OriginalScore = result.OriginalScore,
        FinalScore = result.FinalScore,
        IsEditedByTeacher = result.IsEditedByTeacher
    };
}
