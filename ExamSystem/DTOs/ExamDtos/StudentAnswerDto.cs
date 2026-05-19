namespace ExamSystem.DTOs.ExamDtos;

public class StudentAnswerDto
{
    public int QuestionId { get; set; }
    public List<string> SelectedOptions { get; set; } = new();
}