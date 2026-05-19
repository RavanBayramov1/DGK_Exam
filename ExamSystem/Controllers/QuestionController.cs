using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController(IQuestionService _questionService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _questionService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _questionService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok(result.Data);
    }

    [HttpGet("exam/{examId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByExamId(int examId)
    {
        var result = await _questionService.GetByExamIdAsync(examId);
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateQuestionDto dto)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _questionService.CreateAsync(dto, teacherId);
        if (!result.IsSuccess)
            return BadRequest(result.Error!.Description);

        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateQuestionDto dto)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _questionService.UpdateAsync(id, dto, teacherId);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                ErrorType.Unauthorized => Unauthorized(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _questionService.DeleteAsync(id, teacherId);
        if (!result.IsSuccess)
            return result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error.Description),
                ErrorType.Unauthorized => Unauthorized(result.Error.Description),
                _ => BadRequest(result.Error.Description)
            };

        return NoContent();
    }
}