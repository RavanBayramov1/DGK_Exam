using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class UserRepository(AddDbContext _context) : GenericRepository<User>(_context), IUserRepository
{
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
    }
}
