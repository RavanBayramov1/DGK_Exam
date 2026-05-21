using ExamSystem.Common;
using ExamSystem.DTOs.GroupDtos;

namespace ExamSystem.Services.Interfaces;

public interface IGroupService
{
    Task<ServiceResult<List<GroupResponseDto>>> GetAllAsync();
    Task<ServiceResult<GroupDetailDto>> GetByIdAsync(int id);
    Task<ServiceResult<GroupResponseDto>> CreateAsync(CreateGroupDto dto);
    Task<ServiceResult<GroupResponseDto>> UpdateAsync(int id, UpdateGroupDto dto);
    Task<ServiceResult> DeleteAsync(int id);
    Task<ServiceResult> AddStudentAsync(int groupId, int studentId);     
    Task<ServiceResult> RemoveStudentAsync(int groupId, int studentId);   
    Task<ServiceResult> AddTeacherAsync(int groupId, int teacherId);      
    Task<ServiceResult> RemoveTeacherAsync(int groupId, int teacherId);   
}