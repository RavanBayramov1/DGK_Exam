using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.ResultDtos;
using ExamSystem.Enums;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResultController(IResultService _resultService) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _resultService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _resultService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpGet("exam/{examId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByExamId(int examId)
    {
        var result = await _resultService.GetByExamIdAsync(examId);
        return Ok(result.Data);
    }

    [HttpGet("student/{studentId}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetByStudentId(int studentId)
    {
        var result = await _resultService.GetByStudentIdAsync(studentId);
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateResultDto dto)
    {
        var result = await _resultService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Nəticə uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _resultService.DeleteAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return NoContent();
    }
}
