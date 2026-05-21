using ExamSystem.Common;
using ExamSystem.DTOs.GroupDtos;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExamSystem.Services.Implementations;

public class GroupService(IGroupRepository _groupRepo,IUserRepository _userRepo) : IGroupService
{
    public async Task<ServiceResult<List<GroupResponseDto>>> GetAllAsync()
    {
        var groups = await _groupRepo.GetAllAsync();
        var result = groups.Select(g => (GroupResponseDto)g).ToList();
        return ServiceResult<List<GroupResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<GroupDetailDto>> GetByIdAsync(int id)
    {
        var group = await _groupRepo.GetWithDetailsAsync(id);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        return ServiceResult<GroupDetailDto>.Success(group);
    }

    public async Task<ServiceResult<GroupResponseDto>> CreateAsync(CreateGroupDto dto)
    {
        Group group = dto;
        await _groupRepo.AddAsync(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult<GroupResponseDto>.Success(group);
    }

    public async Task<ServiceResult<GroupResponseDto>> UpdateAsync(int id, UpdateGroupDto dto)
    {
        var group = await _groupRepo.GetByIdAsync(id);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        group.Name = dto.Name;

        _groupRepo.Update(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult<GroupResponseDto>.Success(group);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var group = await _groupRepo.GetByIdAsync(id);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        _groupRepo.SoftDelete(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
    public async Task<ServiceResult> AddStudentAsync(int groupId, int studentId)
    {
        var group = await _groupRepo.GetWithDetailsAsync(groupId);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        var student = await _userRepo.GetByIdAsync(studentId);
        if (student is null)
            return ErrorMessages.User.NotFound;

        if (student.Role != UserRole.Student)
            return ErrorMessages.User.NotStudent;

        if (group.Students.Any(s => s.Id == studentId))
            return ErrorMessages.User.AlreadyInGroup;

        student.GroupId = groupId;
        _userRepo.Update(student);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RemoveStudentAsync(int groupId, int studentId)
    {
        var group = await _groupRepo.GetWithDetailsAsync(groupId);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        var student = group.Students.FirstOrDefault(s => s.Id == studentId);
        if (student is null)
            return ErrorMessages.User.NotInGroup;

        student.GroupId = null;
        _userRepo.Update(student);
        await _userRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> AddTeacherAsync(int groupId, int teacherId)
    {
        var group = await _groupRepo.GetWithDetailsAsync(groupId);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        var teacher = await _userRepo.GetByIdAsync(teacherId);
        if (teacher is null)
            return ErrorMessages.User.NotFound;

        if (teacher.Role != UserRole.Teacher)
            return ErrorMessages.User.NotTeacher;

        if (group.Teachers.Any(t => t.Id == teacherId))
            return ErrorMessages.User.AlreadyInGroup;

        group.Teachers.Add(teacher);
        _groupRepo.Update(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RemoveTeacherAsync(int groupId, int teacherId)
    {
        var group = await _groupRepo.GetWithDetailsAsync(groupId);
        if (group is null)
            return ErrorMessages.Group.NotFound;

        var teacher = group.Teachers.FirstOrDefault(t => t.Id == teacherId);
        if (teacher is null)
            return ErrorMessages.User.NotInGroup;

        group.Teachers.Remove(teacher);
        _groupRepo.Update(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
