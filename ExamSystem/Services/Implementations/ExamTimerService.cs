using ExamSystem.DTOs.ExamDtos;
using ExamSystem.Hubs;
using ExamSystem.Repositories.Interfaces;
using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ExamSystem.Services.Implementations;

public class ExamTimerService (IServiceScopeFactory _scopeFactory,
IHubContext<ExamHub> _hubContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var resultRepo = scope.ServiceProvider.GetRequiredService<IResultRepository>();
            var examService = scope.ServiceProvider.GetRequiredService<IExamService>();

            var activeResults = await resultRepo.GetActiveResultsAsync();

            foreach (var result in activeResults)
            {
                if (result.Exam is null) continue; // ✅ artıq include edilib

                var endTime = result.StartedAt!.Value.AddMinutes(result.Exam.DurationMinutes);
                var remaining = endTime - DateTime.UtcNow;

                if (remaining <= TimeSpan.Zero)
                {
                    await _hubContext.Clients
                        .User(result.StudentId.ToString())
                        .SendAsync("TimeUp");

                    await examService.AutoSubmitExamAsync(result.Id);
                }
                else
                {
                    await _hubContext.Clients
                        .User(result.StudentId.ToString())
                        .SendAsync("TimeRemaining", (int)remaining.TotalSeconds);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
