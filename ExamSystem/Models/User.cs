using ExamSystem.Enums;
using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public ICollection<Result> Results { get; set; } = new List<Result>();
}
