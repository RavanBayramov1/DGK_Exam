using ExamSystem.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Models;

public class ExamResult : BaseEntity
{
    public int ExamId { get; set; }
    public Exam Exam { get; set; }

    public int StudentId { get; set; }
    public AppUser Student { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsAutoSubmitted { get; set; }

    public decimal OriginalScore { get; set; }
    public decimal FinalScore { get; set; }
    public bool IsEditedByTeacher { get; set; }

    [Column(TypeName = "jsonb")]
    public List<StudentAnswerData> GivenAnswers { get; set; }
}

// JSON Struktur (Bazada cədvəl deyil)
public class StudentAnswerData
{
    public int QuestionId { get; set; }
    public List<string> SelectedOptions { get; set; }

    public List<string> CorrectOptions { get; set; }
    public bool? IsCorrect { get; set; } // Açıq suallar üçün nullable
    public decimal PointsEarned { get; set; }
    public string TeacherFeedback { get; set; }
}