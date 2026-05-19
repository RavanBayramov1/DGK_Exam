using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implementations;

public class GroupRepository : GenericRepository<Group>, IGroupRepository
{
    public GroupRepository(AddDbContext context) : base(context) { }

    public async Task<Group?> GetWithDetailsAsync(int id) =>
        await _dbSet
            .Include(g => g.Students)
            .Include(g => g.Teachers)
            .FirstOrDefaultAsync(g => g.Id == id);

    public async Task<List<Group>> GetByTeacherIdAsync(int teacherId) =>
        await _dbSet
            .Where(g => g.Teachers.Any(t => t.Id == teacherId))
            .ToListAsync();
}
