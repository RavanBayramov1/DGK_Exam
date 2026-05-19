using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class ResultRepository : GenericRepository<ExamResult>, IResultRepository
{
    public ResultRepository(AddDbContext context) : base(context) { }

    public async Task<ExamResult?> GetByExamAndStudentAsync(int examId, int studentId) =>
        await _dbSet
            .Include(r => r.Exam)
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.ExamId == examId && r.StudentId == studentId);

    public async Task<List<ExamResult>> GetByStudentIdAsync(int studentId) =>
        await _dbSet
            .Where(r => r.StudentId == studentId)
            .Include(r => r.Exam)
            .ToListAsync();

    public async Task<List<ExamResult>> GetByExamIdAsync(int examId) =>
        await _dbSet
            .Where(r => r.ExamId == examId)
            .Include(r => r.Student)
            .ToListAsync();
}
