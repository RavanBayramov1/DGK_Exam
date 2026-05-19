using ExamSystem.Data;
using ExamSystem.Enums;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Services.Implementations;

public class ExamStatusBackgroundService(IServiceScopeFactory _scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var examRepo = scope.ServiceProvider.GetRequiredService<IExamRepository>();

            var now = DateTime.UtcNow;
            var exams = await examRepo.GetAllAsync();

            foreach (var exam in exams)
            {
                var endTime = exam.StartTime.AddMinutes(exam.DurationMinutes);

                if (exam.Status == ExamStatus.Scheduled && exam.StartTime <= now)
                {
                    exam.Status = ExamStatus.Active;
                    examRepo.Update(exam);
                }
                else if (exam.Status == ExamStatus.Active && endTime <= now)
                {
                    exam.Status = ExamStatus.Ended;
                    examRepo.Update(exam);
                }
            }

            await examRepo.SaveChangesAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
