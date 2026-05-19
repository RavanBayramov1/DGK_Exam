using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IExamQuestionRepository : IGenericRepository<ExamQuestion>
{
    Task<List<ExamQuestion>> GetByExamIdAsync(int examId);
}