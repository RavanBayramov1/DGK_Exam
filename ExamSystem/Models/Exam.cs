using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class Exam : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateTime StartTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public int UserId { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Result> Results { get; set; } = new List<Result>();
}
