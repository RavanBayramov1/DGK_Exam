using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<List<Question>> GetByExamIdAsync(int examId);
}
