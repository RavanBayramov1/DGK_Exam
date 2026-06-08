using ExamSystem.Data;
using ExamSystem.Enums;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implementations;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext context) : base(context) { }

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
    public async Task<List<Exam>> GetAllWithDetailsAsync() =>
    await _dbSet
        .Include(e => e.Group)
        .Include(e => e.Subject)
        .Include(e => e.Teacher)
        .ToListAsync();
    public async Task<List<Exam>> GetActiveAndScheduledAsync() =>
        await _dbSet
            .Where(e => !e.IsDeleted &&
                   (e.Status == ExamStatus.Draft ||
                    e.Status == ExamStatus.Scheduled ||
                    e.Status == ExamStatus.Active))
            .ToListAsync();
}
