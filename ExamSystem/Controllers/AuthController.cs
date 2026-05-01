using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ApiControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Qeydiyyat uğurlu oldu.");
    }

    [HttpPut("Login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }
    [HttpDelete("LogOut")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");
        var result = await _authService.LogoutAsync(token);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Çıxış uğurlu oldu.");
    }
}
