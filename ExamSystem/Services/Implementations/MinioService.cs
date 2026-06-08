using ExamSystem.Common;
using Minio;
using Minio.Exceptions;

namespace ExamSystem.Services.Implementations;

public class MinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioService(IConfiguration configuration)
    {
        _bucketName = configuration["bucket"] ?? "exam-system-bucket";

        var minio = configuration.GetSection("Minio");
        bool secure = bool.Parse(minio["Secure"] ?? "false");

        _minioClient = new MinioClient()
            .WithEndpoint(minio["Endpoint"])
            .WithCredentials(minio["AccessKey"], minio["SecretKey"])
            .WithSSL(secure)
            .Build();
    }

    public async Task<ServiceResult<string>> UploadFileAsync(IFormFile file, string subPath, string fileName)
    {
        try
        {
            bool bucketExists = await _minioClient
                .BucketExistsAsync(new Minio.DataModel.Args.BucketExistsArgs().WithBucket(_bucketName));

            if (!bucketExists)
                await _minioClient.MakeBucketAsync(new Minio.DataModel.Args.MakeBucketArgs().WithBucket(_bucketName));

            var fullPath = $"{subPath}/{fileName}";

            var deleteResult = await DeleteFileAsync(fullPath);
            if (!deleteResult.IsSuccess)
                return ServiceResult<string>.Failure(deleteResult.Error!);

            using var stream = file.OpenReadStream();
            await _minioClient.PutObjectAsync(new Minio.DataModel.Args.PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fullPath)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType));

            return ServiceResult<string>.Success(fullPath);
        }
        catch 
        {
            return ErrorMessages.Minio.UploadFailed;
        }
    }

    public async Task<ServiceResult<string>> GetPresignedUrlAsync(string fullPath)
    {
        try
        {
            await _minioClient.StatObjectAsync(new Minio.DataModel.Args.StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fullPath));

            var url = await _minioClient.PresignedGetObjectAsync(new Minio.DataModel.Args.PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fullPath)
                .WithExpiry(3600));

            return ServiceResult<string>.Success(url);
        }
        catch (ObjectNotFoundException) 
        {
            return ErrorMessages.Minio.NotFound;
        }
        catch
        {
            return ErrorMessages.Minio.GetFailed;
        }
    }

    public async Task<ServiceResult> DeleteFileAsync(string fullPath)
    {
        try
        {
            bool exists = await FileExistsAsync(fullPath);
            if (!exists) return ServiceResult.Success();

            await _minioClient.RemoveObjectAsync(new Minio.DataModel.Args.RemoveObjectArgs().WithBucket(_bucketName).WithObject(fullPath));
            return ServiceResult.Success();
        }
        catch
        {
            return ErrorMessages.Minio.DeleteFailed;
        }
    }
    public async Task<bool> FileExistsAsync(string fullPath)
    {
        try
        {
            await _minioClient.StatObjectAsync(new Minio.DataModel.Args.StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fullPath));
            return true;
        }
        catch(ObjectNotFoundException) 
        {
            return false;
        }
    } 
}
