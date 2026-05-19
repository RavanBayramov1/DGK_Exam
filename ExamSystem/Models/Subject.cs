using ExamSystem.Models.Common;

namespace ExamSystem.Models;

public class Subject : BaseEntity
{
    public string Name { get; set; }

    public ICollection<Group> Groups { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<Exam> Exams { get; set; }
}
