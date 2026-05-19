using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class UserService(IUserRepository _userRepo) : IUserService
{
    public async Task<ServiceResult<List<UserSummaryDto>>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var result = users.Select(u => (UserSummaryDto)u).ToList();
        return ServiceResult<List<UserSummaryDto>>.Success(result);
    }

    public async Task<ServiceResult<UserSummaryDto>> GetByIdAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user is null)
            return Error.NotFound("İstifadəçi tapılmadı.");

        return ServiceResult<UserSummaryDto>.Success(user);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user is null)
            return Error.NotFound("İstifadəçi tapılmadı.");

        _userRepo.SoftDelete(user);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> AssignRoleAsync(int userId, AssignRoleDto dto)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user is null)
            return Error.NotFound("İstifadəçi tapılmadı.");

        if (dto.Role == UserRole.Student && dto.GroupId is null)
            return Error.Validation("Student üçün GroupId mütləqdir.");

        user.Role = dto.Role;
        user.GroupId = dto.Role == UserRole.Student ? dto.GroupId : null;

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> ResetPasswordAsync(int userId, ResetPasswordDto dto)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user is null)
            return Error.NotFound("İstifadəçi tapılmadı.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
