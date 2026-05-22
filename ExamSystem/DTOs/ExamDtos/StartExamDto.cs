using ExamSystem.DTOs.QuestionDtos;

namespace ExamSystem.DTOs.ExamDtos;

public class StartExamDto
{
    public int ResultId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }  // ← əvvəl Duration idi
    public List<QuestionResponseDto> Questions { get; set; } = new();
}
