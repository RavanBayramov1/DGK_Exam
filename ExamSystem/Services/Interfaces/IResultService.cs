using ExamSystem.Common;
using ExamSystem.DTOs.ResultDtos;

namespace ExamSystem.Services.Interfaces;

public interface IResultService
{
    Task<ServiceResult<List<ResultResponseDto>>> GetAllAsync();
    Task<ServiceResult<ResultResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<ResultResponseDto>>> GetByExamIdAsync(int examId);
    Task<ServiceResult<List<ResultResponseDto>>> GetByStudentIdAsync(int studentId);
    Task<ServiceResult> CreateAsync(CreateResultDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateResultDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
