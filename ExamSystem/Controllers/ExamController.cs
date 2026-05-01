using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExamController(IExamService _examService) : ApiControllerBase
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _examService.GetAllAsync();
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _examService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpPost("CreateExam")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateExamDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.CreateAsync(dto, userId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("İmtahan yaradıldı.");
    }
    [HttpPut("Update")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateExamDto dto)
    {
        var result = await _examService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("İmtahan yeniləndi.");
    }

    ///////////////////////////////////

    [HttpPost("{examId}/Start")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> StartExam(int examId)
    {
        var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.StartExamAsync(examId, studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpPost("Submit")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitExam(SubmitExamDto dto)
    {
        var result = await _examService.SubmitExamAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("İmtahan uğurla təhvil verildi.");
    }

    /////////////////////////////////

    [HttpDelete("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _examService.DeleteAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("İmtahan silindi.");
    }
}
