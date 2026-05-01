using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;

namespace ExamSystem.Services.Interfaces;

public interface IExamService
{
    Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync();
    Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult> CreateAsync(CreateExamDto dto, int userrId);
    Task<ServiceResult> UpdateAsync(int id, UpdateExamDto dto);
    Task<ServiceResult<StartExamDto>> StartExamAsync(int examId, int studentId);
    Task<ServiceResult> SubmitExamAsync(SubmitExamDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
