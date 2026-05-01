using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.ResultDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        if(!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _resultService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("Exam/ExamId")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByExamId(int examId)
    {
        var result = await _resultService.GetByExamIdAsync(examId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpGet("Student/StudentId")]
    public async Task<IActionResult> GetByStudentId(int studentId)
    {
        var result = await _resultService.GetByStudentIdAsync(studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateResultDto dto)
    {
        var result = await _resultService.CreateAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Nəticə yaradıldı.");
    }

    [HttpPatch("{Id}/Status")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update(int id, UpdateResultDto dto)
    {
        var result = await _resultService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Nəticə yeniləndi.");
    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _resultService.DeleteAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Nəticə ləğv edildi.");
    }

}
