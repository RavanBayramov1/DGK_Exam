using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController(IQuestionService _questionService) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _questionService.GetAllAsync();
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _questionService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("exam/{examId}")]
    public async Task<IActionResult> GetByExamId(int examId)
    {
        var result = await _questionService.GetByExamIdAsync(examId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateQuestionDto dto)
    {
        var result = await _questionService.CreateAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Sual yaradıldı.");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateQuestionDto dto)
    {
        var result = await _questionService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Sual yeniləndi.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _questionService.DeleteAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Sual silindi.");
    }
}
