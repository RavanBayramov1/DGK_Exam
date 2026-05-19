using ExamSystem.DTOs.GroupDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController(IGroupService _groupService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _groupService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _groupService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateGroupDto dto)
    {
        var result = await _groupService.CreateAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result.Error!.Description);

        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateGroupDto dto)
    {
        var result = await _groupService.UpdateAsync(id, dto);
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
        var result = await _groupService.DeleteAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return NoContent();
    }
}
