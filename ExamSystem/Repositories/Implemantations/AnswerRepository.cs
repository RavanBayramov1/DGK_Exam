using ExamSystem.Data;
using ExamSystem.Models;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(AddDbContext context) : base(context) { }

    public async Task<List<Answer>> GetByResultIdAsync(int resultId)
    {
        return await _dbSet.Where(a => a.ResultId == resultId).ToListAsync();
    }
}