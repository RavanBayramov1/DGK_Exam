using ExamSystem.Common;
using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Qeydiyyat uğurla tamamlandı.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var result = await _authService.LogoutAsync(token);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Çıxış uğurla tamamlandı.");
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
    {
        var result = await _authService.ForgetPasswordAsync(dto.Email);

        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result);
    }

    [HttpPost("reset-password-with-token")]
    public async Task<IActionResult> ResetPasswordWithToken([FromBody] ResetPasswordWithTokenDto dto)
    {
        var result = await _authService.ResetPasswordWithTokenAsync(dto);

        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result);
    }
}

