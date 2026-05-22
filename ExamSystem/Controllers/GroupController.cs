using ExamSystem.Controllers.Base;
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
public class GroupController(IGroupService _groupService) : ApiControllerBase
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
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateGroupDto dto)
    {
        var result = await _groupService.CreateAsync(dto);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateGroupDto dto)
    {
        var result = await _groupService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _groupService.DeleteAsync(id);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return NoContent();
    }

    [HttpPost("{groupId}/add-student/{studentId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddStudent(int groupId, int studentId)
    {
        var result = await _groupService.AddStudentAsync(groupId, studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Tələbə qrupa uğurla əlavə edildi.");
    }

    [HttpDelete("{groupId}/remove-student/{studentId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveStudent(int groupId, int studentId)
    {
        var result = await _groupService.RemoveStudentAsync(groupId, studentId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Tələbə qrupdan uğurla çıxarıldı.");
    }

    [HttpPost("{groupId}/add-teacher/{teacherId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTeacher(int groupId, int teacherId)
    {
        var result = await _groupService.AddTeacherAsync(groupId, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Müəllim qrupa uğurla əlavə edildi.");
    }

    [HttpDelete("{groupId}/remove-teacher/{teacherId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveTeacher(int groupId, int teacherId)
    {
        var result = await _groupService.RemoveTeacherAsync(groupId, teacherId);
        if (!result.IsSuccess)
            return HandleFailure(result);

        return Ok("Müəllim qrupdan uğurla çıxarıldı.");
    }
}

