using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(AddDbContext context) : base(context) { }

    public async Task<List<Question>> GetByTeacherIdAsync(int teacherId) =>
        await _dbSet
            .Where(q => q.TeacherId == teacherId)
            .Include(q => q.Subject)
            .ToListAsync();

    public async Task<List<Question>> GetBySubjectIdAsync(int subjectId) =>
        await _dbSet
            .Where(q => q.SubjectId == subjectId)
            .Include(q => q.Subject)
            .ToListAsync();
    public async Task<List<Question>> GetByExamIdAsync(int examId) =>
    await _dbSet
        .Where(q => q.ExamQuestions.Any(eq => eq.ExamId == examId))
        .Include(q => q.Subject)
        .ToListAsync();
}
