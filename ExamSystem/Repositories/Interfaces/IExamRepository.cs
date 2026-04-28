using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IExamRepository : IGenericRepository<Exam>
{
    Task<List<Exam>> GetByUserIdAsync(int userId);
}
