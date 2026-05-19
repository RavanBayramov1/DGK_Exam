
using ExamSystem.Data;
using ExamSystem.Extensions;
using ExamSystem.Middlewares;
using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExamSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // DbContext
        builder.Services.AddDbContext<AddDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Redis
        builder.Services.AddRedis(builder.Configuration);

        // Repositories
        builder.Services.AddRepositories();

        // Services
        builder.Services.AddServices();

        // BackgroundService
        builder.Services.AddBackgroundServices();

        // JWT
        builder.Services.AddJwt(builder.Configuration);

        // Swagger
        builder.Services.AddSwagger(); 


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseMiddleware<TokenBlacklistMiddleware>();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            AdminSeeder.Seed(scope.ServiceProvider);
        }

        app.MapControllers();
        app.Run();
    }
}
