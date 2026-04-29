using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AddDbContext context) : base(context) { }

    public async Task<List<Exam>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(e => e.UserId == userId).ToListAsync();
    }
}
