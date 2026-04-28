using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ResultDtos;

public class CreateResultDto
{
    [Required(ErrorMessage = "İmtahan boş ola bilməz.")]
    public int ExamId { get; set; }

    [Required(ErrorMessage = "Tələbə boş ola bilməz.")]
    public int StudentId { get; set; }


    public static implicit operator Result(CreateResultDto dto) => new()
    {
        ExamId = dto.ExamId,
        StudentId = dto.StudentId,
        Score = 0,
        Status = "pending",
        TakenAt = DateTime.UtcNow
    };
}
