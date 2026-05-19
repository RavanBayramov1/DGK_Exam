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
public class AuthController(IAuthService _authService,ITokenBlacklistService _blacklistService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.Conflict => Conflict(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok("Qeydiyyat uğurla tamamlandı.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.Unauthorized => Unauthorized(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok(result.Data);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ServiceResult> LogoutAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiry = jwtToken.ValidTo - DateTime.UtcNow;

        if (expiry <= TimeSpan.Zero)
            return ServiceResult.Success(); // Token artıq expire olub, blacklist lazım deyil

        await _blacklistService.AddToBlacklistAsync(token, expiry);
        return ServiceResult.Success();
    }
    //public async Task<IActionResult> Logout()
    //{
    //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //    var result = await _authService.LogoutAsync(token);
    //    if (!result.IsSuccess)
    //        return BadRequest(result.Error!.Description);

    //    return Ok("Çıxış uğurla tamamlandı.");
    //}
}
