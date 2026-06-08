
using ExamSystem.Data;
using ExamSystem.Extensions;
using ExamSystem.Hubs;
using ExamSystem.Middlewares;
using ExamSystem.Seeds;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
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


        //SignalR
        builder.Services.AddSignalR();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

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
        app.MapHub<ExamHub>("/hubs/exam");

        app.UseCors("AllowAll");

        using (var scope = app.Services.CreateScope())
        {
            AdminSeeder.Seed(scope.ServiceProvider);
        }

        app.MapControllers();
        app.Run();
    }
}
