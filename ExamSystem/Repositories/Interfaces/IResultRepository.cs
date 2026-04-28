using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IResultRepository : IGenericRepository<Result>
{
    Task<List<Result>> GetByExamIdAsync(int examId);
    Task<List<Result>> GetByStudentIdAsync(int studentId);
}
