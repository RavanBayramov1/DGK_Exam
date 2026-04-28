using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ExamDtos;

public class UpdateExamDto
{
    [Required(ErrorMessage = "Başlıq boş ola bilməz.")]
    [MinLength(3, ErrorMessage = "Başlıq minimum 3 simvol olmalıdır.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 300, ErrorMessage = "Müddət 1-300 dəqiqə arasında olmalıdır.")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Başlama vaxtı boş ola bilməz.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Status boş ola bilməz.")]
    [RegularExpression("active|inactive|finished", ErrorMessage = "Status active, inactive və ya finished olmalıdır.")]
    public string Status { get; set; } = string.Empty;


    public static implicit operator Exam(UpdateExamDto dto) => new()
    {
        Title = dto.Title,
        Duration = dto.Duration,
        StartTime = dto.StartTime,
        Status = dto.Status
    };
}
