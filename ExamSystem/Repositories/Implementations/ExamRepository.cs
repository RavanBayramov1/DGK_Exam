using ExamSystem.Data;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AddDbContext context) : base(context) { }

    public async Task<Exam?> GetWithDetailsAsync(int id) =>
        await _dbSet
            .Include(e => e.Group)
            .Include(e => e.Subject)
            .Include(e => e.Teacher)
            .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<List<Exam>> GetByGroupIdAsync(int groupId) =>
        await _dbSet
            .Where(e => e.GroupId == groupId)
            .Include(e => e.Subject)
            .ToListAsync();

    public async Task<List<Exam>> GetByTeacherIdAsync(int teacherId) =>
        await _dbSet
            .Where(e => e.TeacherId == teacherId)
            .Include(e => e.Group)
            .Include(e => e.Subject)
            .ToListAsync();

    public async Task<List<Exam>> GetActiveExamsAsync() =>
        await _dbSet
            .Where(e => e.Status == ExamStatus.Active)
            .Include(e => e.Group)
            .Include(e => e.Subject)
            .ToListAsync();
}
