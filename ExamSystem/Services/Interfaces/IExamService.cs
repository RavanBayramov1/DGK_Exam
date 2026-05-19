using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;

namespace ExamSystem.Services.Interfaces;

public interface IExamService
{
    Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync();
    Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult> CreateAsync(CreateExamDto dto, int teacherId);
    Task<ServiceResult> UpdateAsync(int id, UpdateExamDto dto, int teacherId);
    Task<ServiceResult> DeleteAsync(int id, int teacherId);
    Task<ServiceResult<StartExamDto>> StartExamAsync(int examId, int studentId);
    Task<ServiceResult> SubmitExamAsync(SubmitExamDto dto);
    Task<ServiceResult> AddQuestionToExamAsync(int examId, int questionId, decimal points, int teacherId);
    Task<ServiceResult> RemoveQuestionFromExamAsync(int examId, int questionId, int teacherId);
}