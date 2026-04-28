using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(AddDbContext context) : base(context) { }

    public async Task<List<Question>> GetByExamIdAsync(int examId)
    {
        return await _dbSet.Where(q => q.ExamId == examId).ToListAsync();
    }
}
