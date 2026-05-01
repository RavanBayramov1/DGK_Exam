using ExamSystem.DTOs.QuestionDtos;

namespace ExamSystem.DTOs.ExamDtos;

public class StartExamDto
{
    public int ResultId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public int Duration { get; set; }
    public List<QuestionResponseDto> Questions { get; set; } = new();   
}
