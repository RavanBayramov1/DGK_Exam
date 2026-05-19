using ExamSystem.Common;
using ExamSystem.DTOs.SubjectDtos;

namespace ExamSystem.Services.Interfaces;

public interface ISubjectService
{
    Task<ServiceResult<List<SubjectResponseDto>>> GetAllAsync();
    Task<ServiceResult<SubjectResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<SubjectResponseDto>> CreateAsync(CreateSubjectDto dto);
    Task<ServiceResult<SubjectResponseDto>> UpdateAsync(int id, UpdateSubjectDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
