using ExamSystem.Common;
using Minio;

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

    public async Task<ServiceResult<string>> UploadFileAsync(IFormFile file , string subPath , string fileName)
    {

    }
}
