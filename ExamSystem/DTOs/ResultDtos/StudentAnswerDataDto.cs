namespace ExamSystem.DTOs.ResultDtos;

public class StudentAnswerDataDto
{
    public int QuestionId { get; set; }
    public List<string> SelectedOptions { get; set; } = new(); // ← əlavə edildi
    public bool? IsCorrect { get; set; }
    public decimal PointsEarned { get; set; }
    public string TeacherFeedback { get; set; } = string.Empty;
}
