using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Implemantations;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implementations;

public class ExamQuestionRepository(AddDbContext _context) : GenericRepository<ExamQuestion>(_context), IExamQuestionRepository
{
    public async Task<List<ExamQuestion>> GetByExamIdAsync(int examId) =>
        await _dbSet
            .Where(eq => eq.ExamId == examId)
            .Include(eq => eq.Question)
            .ToListAsync();
}
