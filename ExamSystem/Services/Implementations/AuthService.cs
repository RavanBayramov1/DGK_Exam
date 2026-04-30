using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class AuthService(IUserRepository _userRepository, IJwtService _jwtService, ITokenBlacklistService _tokenBlacklistService) : IAuthService
{

    public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepository.GetByUserNameAsync(dto.UserName);
        if(existing != null)
            return Error.Conflict("Bu istifadəçi adı artıq mövcuddur.");

        var user = new User
        {
            UserName = dto.UserName,
            FullName = dto.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Student,    
        };
        await _userRepository.AddAsync(user);
        return ServiceResult.Success();
    }
    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUserNameAsync(dto.UserName);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Error.Unauthorized("İstifadəçi adı və ya şifrə yanlışdır.");

        var token = _jwtService.GenerateToken(user);
        return new AuthResponseDto
        {
            FullName = user.FullName,
            UserName = user.UserName,
            Role = user.Role.ToString(),
            Token = token
        };
    }
    public async Task<ServiceResult> LogoutAsync(string token)
    {
        await _tokenBlacklistService.AddToBlacklistAsync(token, TimeSpan.FromDays(1));
        return ServiceResult.Success();
    }
}
