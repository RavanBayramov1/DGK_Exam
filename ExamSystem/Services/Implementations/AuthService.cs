using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace ExamSystem.Services.Implementations;

public class AuthService(IEmailService _emailService,IUserRepository _userRepo,IJwtService _jwtService,ITokenBlacklistService _blacklistService) : IAuthService
{
    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return ErrorMessages.User.InvalidCredentials;

        var token = _jwtService.GenerateToken(user);
        AuthResponseDto response = user;
        response.Token = token;

        return ServiceResult<AuthResponseDto>.Success(response);
    }

    public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing is not null)
            return ErrorMessages.User.EmailTaken;

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

    public async Task<ServiceResult> ForgetPasswordAsync(string email)
    {
        var user = await _userRepo.GetByEmailAsync(email.ToLower().Trim());
        if (user is null || user.IsDeleted)
            return ErrorMessages.User.NotFound;

        user.PasswordResetToken = Guid.NewGuid().ToString();
        user.ResetTokenExpireTime = DateTime.UtcNow.AddMinutes(15);

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();

        string resetLink = $"https://examsystem.com/reset-password?email={user.Email}&token={user.PasswordResetToken}";

        string emailBody = $"Parolunuzu sıfırlamaq üçün bu linkə klikləyin: <a href='{resetLink}'>Parolu Sıfırla</a>";

        await _emailService.SendEmailAsync(user.Email, "Parolun Sıfırlanması", emailBody);

        return ServiceResult.Success();
    }
    public async Task<ServiceResult> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email.ToLower().Trim());
        if (user is null || user.IsDeleted)
            return ErrorMessages.User.NotFound;

        if (user.PasswordResetToken != dto.Token || user.ResetTokenExpireTime < DateTime.UtcNow)
        {
            return ErrorMessages.User.WrongToken;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        user.PasswordResetToken = null;
        user.ResetTokenExpireTime = null;

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
