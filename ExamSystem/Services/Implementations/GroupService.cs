using ExamSystem.Common;
using ExamSystem.DTOs.GroupDtos;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class GroupService(IGroupRepository _groupRepo) : IGroupService
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
            return Error.NotFound("Qrup tapılmadı.");

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
            return Error.NotFound("Qrup tapılmadı.");

        group.Name = dto.Name;

        _groupRepo.Update(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult<GroupResponseDto>.Success(group);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var group = await _groupRepo.GetByIdAsync(id);
        if (group is null)
            return Error.NotFound("Qrup tapılmadı.");

        _groupRepo.SoftDelete(group);
        await _groupRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
