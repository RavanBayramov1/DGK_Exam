using ExamSystem.Models.Common;

namespace ExamSystem.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }

    public List<AppUser> Students { get; set; }

    public List<AppUser> Teachers { get; set; }

    public ICollection<Subject> Subjects { get; set; }

    public ICollection<Exam> Exams { get; set; }
}

