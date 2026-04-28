using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class ResultRepository : GenericRepository<Result>, IResultRepository
{
    public ResultRepository(AddDbContext context) : base(context) { }

    public async Task<List<Result>> GetByExamIdAsync(int examId)
    {
        return await _dbSet.Where(r => r.ExamId == examId).ToListAsync();
    }

    public async Task<List<Result>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet.Where(r => r.StudentId == studentId).ToListAsync();
    }
}
