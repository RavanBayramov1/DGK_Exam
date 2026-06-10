using ExamSystem.Common;
using ExamSystem.DTOs.QuestionDtos;
using ExamSystem.Models;
using ExamSystem.Services.Interfaces;
using ExamSystem.Repositories.Interfaces;
using System.Reflection.Metadata;
using ExamSystem.Extensions;

namespace ExamSystem.Services.Implementations;

public class QuestionService(IQuestionRepository _questionRepo, MinioService _minioService) : IQuestionService
{
    private const string SubPath = "questions";
public async Task<ServiceResult<List<QuestionResponseDto>>> GetAllAsync(int teacherId)
{
    var questions = await _questionRepo.GetByTeacherIdAsync(teacherId);
    var tasks = questions.Select(async q =>
    {
        var dto = (QuestionResponseDto)q;
        dto.MediaUrl = await GetPresignedUrlOrNull(q.MediaPath);
        return dto;
    });
    var result = await Task.WhenAll(tasks);
    return ServiceResult<List<QuestionResponseDto>>.Success(result.ToList());
}

    public async Task<ServiceResult<QuestionDetailDto>> GetByIdAsync(int id)
    {
        var question = await _questionRepo.GetByIdWithDetailAsync(id);
        if (question is null)
            return ErrorMessages.Question.NotFound;

        var dto = (QuestionDetailDto)question;
        dto.MediaUrl = await GetPresignedUrlOrNull(question.MediaPath);

        return ServiceResult<QuestionDetailDto>.Success(dto);
    }

    public async Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId)
    {
        var questions = await _questionRepo.GetByExamIdAsync(examId);
        var tasks = questions.Select(async q =>
        {
            var dto = (QuestionResponseDto)q;
            dto.MediaUrl = await GetPresignedUrlOrNull(q.MediaPath);
            return dto;
        });
        var result = await Task.WhenAll(tasks);
        return ServiceResult<List<QuestionResponseDto>>.Success(result.ToList());
    }

    public async Task<ServiceResult<QuestionResponseDto>> CreateAsync(CreateQuestionDto dto, int teacherId)
    {
        Question question = dto;
        question.TeacherId = teacherId;
        
        await _questionRepo.AddAsync(question);
        await _questionRepo.SaveChangesAsync();

        if (dto.MediaFile is not null)
        {
            var uploadResult = await UploadMediaAsync(dto.MediaFile, question.Id);
            if (!uploadResult.IsSuccess)
                return ErrorMessages.Question.MediaUploadFailed;
            question.MediaPath = uploadResult.Data;
            _questionRepo.Update(question);
            await _questionRepo.SaveChangesAsync();

        }

        question = await _questionRepo.GetByIdAsync(question.Id);

        var responseDto = (QuestionResponseDto)question;
        responseDto.MediaUrl = await GetPresignedUrlOrNull(question.MediaPath);
        return ServiceResult<QuestionResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<QuestionResponseDto>> UpdateAsync(int id, UpdateQuestionDto dto, int teacherId)
    {
        var question = await _questionRepo.GetByIdWithDetailAsync(id);
        if (question is null)
            return ErrorMessages.Question.NotFound;

        if (question.TeacherId != teacherId)
            return ErrorMessages.Question.Unauthorized;

        question.QuestionText = dto.QuestionText;
        question.Type = dto.Type;
        question.DefaultPoints = dto.DefaultPoints;
        question.Options = dto.Options;
        question.CorrectAnswers = dto.CorrectAnswers;
        question.SubjectId = dto.SubjectId;

        if(dto.MediaFile is not null)
        {
            if (question.MediaPath is not null)
            {
                var deleteresult = await _minioService.DeleteFileAsync(question.MediaPath);
                if (!deleteresult.IsSuccess)
                    return ErrorMessages.Question.MediaDeleteFailed;
            }
            var uploadResult = await UploadMediaAsync(dto.MediaFile, question.Id);
            if (!uploadResult.IsSuccess)
                return ErrorMessages.Question.MediaUploadFailed;

            question.MediaPath = uploadResult.Data;
        }

        _questionRepo.Update(question);
        await _questionRepo.SaveChangesAsync();

        question = await _questionRepo.GetByIdWithDetailAsync(id);

        var responseDto = (QuestionResponseDto)question;
        responseDto.MediaUrl = await GetPresignedUrlOrNull(question.MediaPath);
        return ServiceResult<QuestionResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id, int teacherId)
    {
        var question = await _questionRepo.GetByIdAsync(id);
        if (question is null)
            return ErrorMessages.Question.NotFound;

        if (question.TeacherId != teacherId)
            return ErrorMessages.Question.Unauthorized;

        if (question.MediaPath is not null)
        {
            var deleteResult = await _minioService.DeleteFileAsync(question.MediaPath);
            if (!deleteResult.IsSuccess)
                return ErrorMessages.Question.MediaDeleteFailed;
        }

        _questionRepo.SoftDelete(question);
        await _questionRepo.SaveChangesAsync();

        return ServiceResult.Success();
    }

    private async Task<string?> GetPresignedUrlOrNull(string? mediaPath)
    {
        if (string.IsNullOrEmpty(mediaPath))
            return null;
        var result = await _minioService.GetPresignedUrlAsync(mediaPath);
        return result.IsSuccess ? result.Data : null;
    }
    private async Task<ServiceResult<string>> UploadMediaAsync(IFormFile file, int questionId)
    {
        if (!file.IsValidType("image/"))
            return ErrorMessages.Question.InvalidFileType;
        if (!file.IsValidSizeMb(15))
            return ErrorMessages.Question.FileTooLarge;
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{questionId}{ext}";
        return await _minioService.UploadFileAsync(file, SubPath, fileName);

    }
}