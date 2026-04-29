using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;

namespace ExamSystem.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult> AssignRoleAsync(int userId, AssignRoleDto dto);
    Task<ServiceResult> ResetPasswordAsync(int userId, ResetPasswordDto dto);
}
