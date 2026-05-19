using ExamSystem.Models.Common;

namespace ExamSystem.Models;

public class ExamQuestion : BaseEntity
{
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; }

    public int OrderIndex { get; set; } // İmtahanda sualın sırası
    public decimal Points { get; set; } // Bu imtahan üçün özəl bal
}
