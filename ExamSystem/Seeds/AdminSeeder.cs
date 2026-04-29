using ExamSystem.Data;
using ExamSystem.Enums;
using ExamSystem.Models;

namespace ExamSystem.Seeds;

public static class AdminSeeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AddDbContext>();

        if (!context.Users.Any(u => u.Role == UserRole.Admin))
        {
            context.Users.Add(new User
            {
                UserName = "admin",
                FullName = "System Admin",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin
            });
            context.SaveChanges();
        }
    }
}
