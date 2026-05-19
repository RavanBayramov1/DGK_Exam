using ExamSystem.Enums;
using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ExamDtos;

public class CreateExamDto
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

    [Required(ErrorMessage = "Qrup boş ola bilməz.")]
    public int GroupId { get; set; }

    [Required(ErrorMessage = "Fənn boş ola bilməz.")]
    public int SubjectId { get; set; }

    public static implicit operator Exam(CreateExamDto dto) => new()
    {
        Title = dto.Title,
        StartTime = dto.StartTime,
        DurationMinutes = dto.DurationMinutes,
        ShuffleQuestions = dto.ShuffleQuestions,
        ShuffleOptions = dto.ShuffleOptions,
        ShowResultsToStudent = dto.ShowResultsToStudent,
        GroupId = dto.GroupId,
        SubjectId = dto.SubjectId,
        Status = ExamStatus.Draft
        // TeacherId controller-də set olunur
    };
}
