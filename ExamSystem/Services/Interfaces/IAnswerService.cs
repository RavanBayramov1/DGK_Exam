using ExamSystem.Common;
using ExamSystem.DTOs.AnswerDtos;

namespace ExamSystem.Services.Interfaces;

public interface IAnswerService
{
    Task<ServiceResult<List<AnswerResponseDto>>> GetByResultIdAsync(int resultId);
    Task<ServiceResult> CreateAsync(CreateAnswerDto dto);
}
