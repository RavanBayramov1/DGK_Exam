using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implementations;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(AppDbContext context) : base(context) { }

    public override async Task<Question?> GetByIdAsync(int id) =>
    await _dbSet
        .Include(q => q.Subject)
        .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

    public async Task<List<Question>> GetByTeacherIdAsync(int teacherId) =>
        await _dbSet
            .Where(q => q.TeacherId == teacherId && !q.IsDeleted)
            .Include(q => q.Subject)
            .ToListAsync();

    public async Task<List<Question>> GetBySubjectIdAsync(int subjectId) =>
        await _dbSet
            .Where(q => q.SubjectId == subjectId && !q.IsDeleted)
            .Include(q => q.Subject)
            .ToListAsync();
    public async Task<List<Question>> GetByExamIdAsync(int examId) =>
    await _dbSet
        .Where(q => q.ExamQuestions.Any(eq => eq.ExamId == examId) && !q.IsDeleted)
        .Include(q => q.Subject)
        .ToListAsync();
    public async Task<List<Question>> GetAllWithDetailAsync() =>
    await _dbSet
        .Include(q => q.Subject)
        .Where(q=> !q.IsDeleted)
        .ToListAsync();

    public async Task<Question?> GetByIdWithDetailAsync(int id) =>
        await _dbSet
            .Include(q => q.Subject)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
}
