using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.ResultDtos;

public class UpdateResultDto
{
    [Range(0, 100, ErrorMessage = "Bal 0 ilə 100 arasında olmalıdır.")]
    public decimal FinalScore { get; set; }

    public List<StudentAnswerDataDto> GivenAnswers { get; set; }
}
