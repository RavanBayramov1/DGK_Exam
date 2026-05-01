using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IUserService _userService) : ApiControllerBase
{
    [HttpPut("assign-role/{userId}")]
    public async Task<IActionResult> AssignRole(int userId, AssignRoleDto dto)
    {
        var result = await _userService.AssignRoleAsync(userId, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Rol uğurla təyin edildi.");
    }

    [HttpPut("reset-password/{userId}")]
    public async Task<IActionResult> ResetPassword(int userId, ResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(userId, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Şifrə uğurla sıfırlandı.");
    }
}
