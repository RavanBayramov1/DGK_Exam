using ExamSystem.Enums;
using ExamSystem.Models.Common;
using System.Text.RegularExpressions;

namespace ExamSystem.Models;

public class AppUser : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    // --- ŞAGİRD ÜÇÜN ---
    public int? GroupId { get; set; }
    public Group Group { get; set; }
    public ICollection<ExamResult> ExamResults { get; set; }

    // --- MÜƏLLİM ÜÇÜN ---
    public ICollection<Group> TaughtGroups { get; set; }
    public ICollection<Question> Questions { get; set; }
}

