using ExamSystem.Common;
using ExamSystem.DTOs.AuthDtos;

namespace ExamSystem.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<ServiceResult> RegisterAsync(RegisterDto dto);
}
