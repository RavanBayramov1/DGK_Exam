using ExamSystem.Common;
using ExamSystem.DTOs.SubjectDtos;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class SubjectService(ISubjectRepository _subjectRepo) : ISubjectService
{
    public async Task<ServiceResult<List<SubjectResponseDto>>> GetAllAsync()
    {
        var subjects = await _subjectRepo.GetAllAsync();
        var result = subjects.Select(s => (SubjectResponseDto)s).ToList();
        return ServiceResult<List<SubjectResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<SubjectResponseDto>> GetByIdAsync(int id)
    {
        var subject = await _subjectRepo.GetByIdAsync(id);
        if (subject is null)
            return ErrorMessages.Subject.NotFound;

        return ServiceResult<SubjectResponseDto>.Success(subject);
    }

    public async Task<ServiceResult<SubjectResponseDto>> CreateAsync(CreateSubjectDto dto)
    {
        bool exists = await _subjectRepo.AnyAsync(s =>
        s.Name.ToLower() == dto.Name.ToLower().Trim());

        if (exists)
            return ServiceResult<SubjectResponseDto>.Failure(ErrorMessages.Subject.AlreadyExists);

        Subject subject = dto;
        await _subjectRepo.AddAsync(subject);
        await _subjectRepo.SaveChangesAsync();

        return ServiceResult<SubjectResponseDto>.Success(subject);
    }

    public async Task<ServiceResult<SubjectResponseDto>> UpdateAsync(int id, UpdateSubjectDto dto)
    {
        var subject = await _subjectRepo.GetByIdAsync(id);
        if (subject is null)
            return ErrorMessages.Subject.NotFound;

        subject.Name = dto.Name;

        _subjectRepo.Update(subject);
        await _subjectRepo.SaveChangesAsync();

        return ServiceResult<SubjectResponseDto>.Success(subject);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var subject = await _subjectRepo.GetByIdAsync(id);
        if (subject is null)
            return ErrorMessages.Subject.NotFound;

        _subjectRepo.SoftDelete(subject);
        await _subjectRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
