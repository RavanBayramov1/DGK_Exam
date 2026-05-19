using ExamSystem.Common;
using ExamSystem.DTOs.ResultDtos;
using static ExamSystem.DTOs.ResultDtos.UpdateResultDto;

namespace ExamSystem.Services.Interfaces;

public interface IResultService
{
    Task<ServiceResult<List<ResultResponseDto>>> GetAllAsync();
    Task<ServiceResult<ResultResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<ResultResponseDto>>> GetByExamIdAsync(int examId);
    Task<ServiceResult<List<ResultResponseDto>>> GetByStudentIdAsync(int studentId);
    Task<ServiceResult> UpdateAsync(int id, UpdateResultDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
