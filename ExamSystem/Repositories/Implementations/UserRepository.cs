using ExamSystem.Data;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class UserRepository : GenericRepository<AppUser>, IUserRepository
{
    public UserRepository(AddDbContext context) : base(context) { }

    public async Task<AppUser?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<AppUser>> GetByRoleAsync(UserRole role) =>
        await _dbSet.Where(u => u.Role == role).ToListAsync();

    public async Task<List<AppUser>> GetByGroupIdAsync(int groupId) =>
        await _dbSet.Where(u => u.GroupId == groupId).ToListAsync();
}
