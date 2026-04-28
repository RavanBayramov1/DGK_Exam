using ExamSystem.Models.Common;
namespace ExamSystem.Models;

public class Answer : BaseEntity
{
    public int ResultId { get; set; }
    public int QuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int EarnedPoint { get; set; }

    public Result Result { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
