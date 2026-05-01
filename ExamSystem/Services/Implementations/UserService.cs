using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class UserService(IUserRepository _userRepository) : IUserService
{
    public async Task<ServiceResult> AssignRoleAsync(int userId, AssignRoleDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Error.NotFound("İstifadəçi tapılmadı!");

        user.Role = Enum.Parse<UserRole>(dto.Role);
        await _userRepository.UpdateAsync(user);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> ResetPasswordAsync(int userId, ResetPasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Error.NotFound("İstifadəçi tapılmadı!");

        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return ServiceResult.Success();
    }
}
