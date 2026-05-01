using ExamSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Services.Implementations;

public class ExamStatusBackgroundService(IServiceProvider _serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AddDbContext>();

            var now = DateTime.UtcNow;
            var exams = await context.Exams.Where(e => !e.IsDeleted).ToListAsync(stoppingToken);

            foreach (var exam in exams)
            {
                if (exam.StartTime <= now && exam.Status == "inactive")
                {
                    exam.Status = "active";
                }
                else if (exam.StartTime.AddMinutes(exam.Duration) <= now && exam.Status == "active")
                {
                    exam.Status = "finished";
                }
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
