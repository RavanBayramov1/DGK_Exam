using ExamSystem.Common;
using ExamSystem.DTOs.QuestionDtos;

namespace ExamSystem.Services.Interfaces;

public interface IQuestionService
{
    Task<ServiceResult<List<QuestionResponseDto>>> GetAllAsync();
    Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId);
    Task<ServiceResult> CreateAsync(CreateQuestionDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateQuestionDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
