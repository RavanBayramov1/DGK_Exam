using ExamSystem.Controllers.Base;
using ExamSystem.DTOs.AnswerDtos;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnswerController(IAnswerService _answerService) : ApiControllerBase
{
    [HttpGet("result/{resultId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByResultId(int resultId)
    {
        var result = await _answerService.GetByResultIdAsync(resultId);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateAnswerDto dto)
    {
        var result = await _answerService.CreateAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);
        return Ok("Cavab əlavə edildi.");
    }
}
