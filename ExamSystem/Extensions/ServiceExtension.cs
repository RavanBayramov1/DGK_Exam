using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Implementations;
using ExamSystem.Services.Interfaces;
using StackExchange.Redis;

namespace ExamSystem.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(config["Redis"]!));
        return services;
    }
}