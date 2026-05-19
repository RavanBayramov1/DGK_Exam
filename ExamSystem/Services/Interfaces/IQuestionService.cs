using ExamSystem.Common;
using ExamSystem.DTOs.QuestionDtos;

namespace ExamSystem.Services.Interfaces;

public interface IQuestionService
{
    Task<ServiceResult<List<QuestionResponseDto>>> GetAllAsync();
    Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId);
    Task<ServiceResult<QuestionResponseDto>> CreateAsync(CreateQuestionDto dto, int teacherId);
    Task<ServiceResult<QuestionResponseDto>> UpdateAsync(int id, UpdateQuestionDto dto, int teacherId);
    Task<ServiceResult> DeleteAsync(int id, int teacherId);
}
