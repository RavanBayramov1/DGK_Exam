using ExamSystem.Models;

namespace ExamSystem.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}
