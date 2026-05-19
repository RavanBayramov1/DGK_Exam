using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implementations;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(AddDbContext context) : base(context) { }

    public async Task<List<Subject>> GetByGroupIdAsync(int groupId) =>
        await _dbSet
            .Where(s => s.Groups.Any(g => g.Id == groupId))
            .ToListAsync();
}
