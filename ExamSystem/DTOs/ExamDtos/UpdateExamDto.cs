using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ExamDtos;

public class UpdateExamDto
{
    [Required(ErrorMessage = "İmtahan adı boş ola bilməz.")]
    [MinLength(3, ErrorMessage = "İmtahan adı minimum 3 simvol olmalıdır.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlama vaxtı boş ola bilməz.")]
    public DateTime StartTime { get; set; }

    [Range(5, 180, ErrorMessage = "Müddət 5 ilə 180 dəqiqə arasında olmalıdır.")]
    public int DurationMinutes { get; set; }

    public bool ShuffleQuestions { get; set; }
    public bool ShuffleOptions { get; set; }
    public bool ShowResultsToStudent { get; set; }

    public int GroupId { get; set; }
    public int SubjectId { get; set; }
}
