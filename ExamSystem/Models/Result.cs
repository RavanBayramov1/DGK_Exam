using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class Result : BaseEntity
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int Score { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime TakenAt { get; set; } = DateTime.UtcNow;

    public Exam Exam { get; set; } = null!;
    public User Student { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
