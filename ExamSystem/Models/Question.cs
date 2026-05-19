using ExamSystem.Enums;
using ExamSystem.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamSystem.Models;

public class Question : BaseEntity
{
    public string QuestionText { get; set; }
    public QuestionType Type { get; set; }
    public decimal DefaultPoints { get; set; }

    [Column(TypeName = "jsonb")]
    public List<string> Options { get; set; } // Variantlar siyahısı
    
    [Column(TypeName = "jsonb")]
    public List<string> CorrectAnswers { get; set; } // Düzgün cavab(lar)

    public int TeacherId { get; set; }
    public AppUser Teacher { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
}
