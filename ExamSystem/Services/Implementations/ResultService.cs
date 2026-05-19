using ExamSystem.Common;
using ExamSystem.DTOs.ResultDtos;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;

namespace ExamSystem.Services.Implementations;

public class ResultService(IResultRepository _resultRepo) : IResultService
{
    public async Task<ServiceResult<List<ResultResponseDto>>> GetAllAsync()
    {
        var results = await _resultRepo.GetAllAsync();
        var result = results.Select(r => (ResultResponseDto)r).ToList();
        return ServiceResult<List<ResultResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<ResultResponseDto>> GetByIdAsync(int id)
    {
        var result = await _resultRepo.GetByIdAsync(id);
        if (result is null)
            return Error.NotFound("Nəticə tapılmadı.");

        return ServiceResult<ResultResponseDto>.Success(result);
    }

    public async Task<ServiceResult<List<ResultResponseDto>>> GetByExamIdAsync(int examId)
    {
        var results = await _resultRepo.GetByExamIdAsync(examId);
        var result = results.Select(r => (ResultResponseDto)r).ToList();
        return ServiceResult<List<ResultResponseDto>>.Success(result);
    }

    public async Task<ServiceResult<List<ResultResponseDto>>> GetByStudentIdAsync(int studentId)
    {
        var results = await _resultRepo.GetByStudentIdAsync(studentId);
        var result = results.Select(r => (ResultResponseDto)r).ToList();
        return ServiceResult<List<ResultResponseDto>>.Success(result);
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateResultDto dto)
    {
        var result = await _resultRepo.GetByIdAsync(id);
        if (result is null)
            return Error.NotFound("Nəticə tapılmadı.");

        result.FinalScore = dto.FinalScore;
        result.IsEditedByTeacher = true;

        if (dto.GivenAnswers is not null)
        {
            foreach (var updatedAnswer in dto.GivenAnswers)
            {
                var answer = result.GivenAnswers
                    .FirstOrDefault(a => a.QuestionId == updatedAnswer.QuestionId);

                if (answer is null) continue;

                answer.IsCorrect = updatedAnswer.IsCorrect;
                answer.PointsEarned = updatedAnswer.PointsEarned;
                answer.TeacherFeedback = updatedAnswer.TeacherFeedback;
            }
        }

        _resultRepo.Update(result);
        await _resultRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var result = await _resultRepo.GetByIdAsync(id);
        if (result is null)
            return Error.NotFound("Nəticə tapılmadı.");

        _resultRepo.SoftDelete(result);
        await _resultRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
