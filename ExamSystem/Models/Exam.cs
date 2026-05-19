using ExamSystem.Enums;
using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class Exam : BaseEntity
{
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public ExamStatus Status { get; set; } = ExamStatus.Draft;

    // ClassTime Funksiyaları
    public bool ShuffleQuestions { get; set; }
    public bool ShuffleOptions { get; set; }
    public bool ShowResultsToStudent { get; set; }

    public int TeacherId { get; set; }
    public AppUser Teacher { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
    public ICollection<ExamResult> Results { get; set; }
}
