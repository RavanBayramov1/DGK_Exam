using ExamSystem.DTOs.SubjectDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectController(ISubjectService _subjectService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _subjectService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _subjectService.GetByIdAsync(id);
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
    public async Task<IActionResult> Create(CreateSubjectDto dto)
    {
        var result = await _subjectService.CreateAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result.Error!.Description);

        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateSubjectDto dto)
    {
        var result = await _subjectService.UpdateAsync(id, dto);
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
        var result = await _subjectService.DeleteAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return NoContent();
    }
}
