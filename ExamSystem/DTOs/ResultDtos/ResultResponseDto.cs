using ExamSystem.Models;

namespace ExamSystem.DTOs.ResultDtos;

public class ResultResponseDto
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int Score { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime TakenAt { get; set; }


    public static implicit operator ResultResponseDto(Result result) => new()
    {
        Id = result.Id,
        ExamId = result.ExamId,
        StudentId = result.StudentId,
        Score = result.Score,
        Status = result.Status,
        TakenAt = result.TakenAt
    };
}
