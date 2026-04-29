using ExamSystem.Common;
using ExamSystem.DTOs.ExamDtos;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class ExamService(IExamRepository _examRepository) : IExamService
{
    public async Task<ServiceResult<List<ExamResponseDto>>> GetAllAsync()
    {
        var exams = await _examRepository.GetAllAsync();
        return exams
            .Where(e => !e.IsDeleted)
            .Select(e => (ExamResponseDto)e)
            .ToList();
    }

    public async Task<ServiceResult<ExamResponseDto>> GetByIdAsync(int id)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if(exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        return (ExamResponseDto)exam;
    }
    public async Task<ServiceResult> CreateAsync(CreateExamDto dto, int userrId)
    {
        Exam exam = dto;
        exam.UserId = userrId;
        await _examRepository.AddAsync(exam);
        return ServiceResult.Success();
    }
    public async Task<ServiceResult> UpdateAsync(int id, UpdateExamDto dto)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if(exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        if(exam.Status == "active")
            return Error.Validation("Aktiv imtahanı redaktə etmək olmaz!");

        exam.Title = dto.Title;
        exam.Duration = dto.Duration;
        exam.StartTime = dto.StartTime;
        exam.Status = dto.Status;
        await _examRepository.UpdateAsync(exam);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var exam = await _examRepository.GetByIdAsync(id);
        if (exam == null || exam.IsDeleted)
            return Error.NotFound("İmtahan tapılmadı!");
        if(exam.Status == "active")
            return Error.Validation("Aktiv imtahanı silmək olmaz!");
        exam.IsDeleted = true;
        await _examRepository.UpdateAsync(exam);
        return ServiceResult.Success();
    }


}
