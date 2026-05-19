using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace ExamSystem.Services.Implementations;

public class AuthService(IUserRepository _userRepo,IJwtService _jwtService,ITokenBlacklistService _blacklistService) : IAuthService
{
    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Error.Unauthorized("Email və ya şifrə yanlışdır.");

        var token = _jwtService.GenerateToken(user);
        AuthResponseDto response = user;
        response.Token = token;

        return ServiceResult<AuthResponseDto>.Success(response);
    }

    public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing is not null)
            return Error.Conflict("Bu email artıq istifadə olunur.");

        AppUser user = dto;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Role = UserRole.Student;

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> LogoutAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiry = jwtToken.ValidTo - DateTime.UtcNow;

        await _blacklistService.AddToBlacklistAsync(token, expiry);
        return ServiceResult.Success();
    }
}
