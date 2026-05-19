using ExamSystem.DTOs.AuthDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return NoContent();
    }

    [HttpPut("{id}/assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(int id, AssignRoleDto dto)
    {
        var result = await _userService.AssignRoleAsync(id, dto);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                ErrorType.Validation => BadRequest(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok("Rol uğurla təyin edildi.");
    }

    [HttpPut("{id}/reset-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ResetPassword(int id, ResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(id, dto);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok("Şifrə uğurla yeniləndi.");
    }
}
