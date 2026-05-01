using ExamSystem.Common;
using ExamSystem.DTOs.ResultDtos;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ExamSystem.Services.Implementations;

public class ResultService(IResultRepository _resultRepository) : IResultService
{
    public async Task<ServiceResult<List<ResultResponseDto>>> GetAllAsync()
    {
        var results = await _resultRepository.GetAllAsync();
        return results
            .Where(r => !r.IsDeleted)
            .Select(r => (ResultResponseDto)r)
            .ToList();
    }
    public async Task<ServiceResult<ResultResponseDto>> GetByIdAsync(int id)
    {
        var result = await _resultRepository.GetByIdAsync(id);
        if (result == null || result.IsDeleted)
            return Error.NotFound("Nəticə tapılmadı!");
        return (ResultResponseDto)result;
    }
    public async Task<ServiceResult<List<ResultResponseDto>>> GetByExamIdAsync(int examId)
    {
        var result = await _resultRepository.GetByExamIdAsync(examId);
        return result 
            .Where(r => !r.IsDeleted)
            .Select (r => (ResultResponseDto)r)
            .ToList();
    }
    public async Task<ServiceResult<List<ResultResponseDto>>> GetByStudentIdAsync(int studentId)
    {
        var result = await _resultRepository.GetByStudentIdAsync(studentId);
        return result
            .Where (r => !r.IsDeleted)
            .Select(r => (ResultResponseDto)r)
            .ToList();
    }
    public async Task<ServiceResult> CreateAsync(CreateResultDto dto)
    {
        var existing = await _resultRepository.GetByStudentIdAsync(dto.StudentId);
        var alredayTaken = existing.Any(r => r.ExamId == dto.ExamId && !r.IsDeleted);
        if (alredayTaken)
            return Error.Conflict("Bu tələbə artıq bu imtahana girib.");

        Models.Result result = dto;
        await _resultRepository.AddAsync(result);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateResultDto dto)
    {
        var result = await _resultRepository.GetByIdAsync(id);
        if (result == null || result.IsDeleted)
            return Error.NotFound("Nəticə tapılmadı!");

        result.Status = dto.Status;
        result.Score = dto.Score;
        await _resultRepository.UpdateAsync(result); 
        return ServiceResult.Success();
    }

    //public async Task<ServiceResult> CalculateScoreAsync(int resultId)
    //{
    //    var result = await _resultRepository.GetByIdAsync(resultId);
    //    if (result == null || result.IsDeleted)
    //        return Error.NotFound("Nəticə tapılmadı!");

    //    var answers = await _answerRepository.GetByResultIdAsync(resultId);
    //    var totalScore = answers.Where(a => !a.IsDeleted).Sum(a => a.EarnedPoint);

    //    result.Score = totalScore;
    //    result.Status = "graded";
    //    await _resultRepository.UpdateAsync(result);
    //    return ServiceResult.Success();
    //}
    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var result = await _resultRepository.GetByIdAsync(id);
        if (result == null || result.IsDeleted)
            return Error.NotFound("Nəticə tapılmadı!");

        result.Score = 0;
        result.Status = "cancelled";
        await _resultRepository.UpdateAsync(result);
        return ServiceResult.Success();
    }

}
