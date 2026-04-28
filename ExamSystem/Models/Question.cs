using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class Question : BaseEntity
{
    public int ExamId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Options { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public int Point { get; set; }

    public Exam Exam { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
