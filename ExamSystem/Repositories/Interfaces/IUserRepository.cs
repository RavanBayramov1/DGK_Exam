using ExamSystem.Enums;
using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<AppUser>
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<List<AppUser>> GetByRoleAsync(UserRole role);
    Task<List<AppUser>> GetByGroupIdAsync(int groupId);
}
