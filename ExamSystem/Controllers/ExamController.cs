using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.Enums;
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
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _examService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _examService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateExamDto dto)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.CreateAsync(dto, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("İmtahan uğurla yaradıldı.");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateExamDto dto)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.UpdateAsync(id, dto, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("İmtahan uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.DeleteAsync(id, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return NoContent();
    }

    [HttpPost("{examId}/start")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> StartExam(int examId)
    {
        var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.StartExamAsync(examId, studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpPost("submit")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitExam(SubmitExamDto dto)
    {
        var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.SubmitExamAsync(dto, studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("İmtahan uğurla təhvil verildi.");
    }

    [HttpPost("{examId}/questions/{questionId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddQuestion(int examId, int questionId, [FromQuery] decimal points)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.AddQuestionToExamAsync(examId, questionId, points, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Sual imtahana uğurla əlavə edildi.");
    }

    [HttpDelete("{examId}/questions/{questionId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> RemoveQuestion(int examId, int questionId)
    {
        var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _examService.RemoveQuestionFromExamAsync(examId, questionId, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return NoContent();
    }
}
