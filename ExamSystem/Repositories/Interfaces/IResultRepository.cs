using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IResultRepository : IGenericRepository<ExamResult>
{
    Task<ExamResult?> GetByExamAndStudentAsync(int examId, int studentId);
    Task<List<ExamResult>> GetByStudentIdAsync(int studentId);
    Task<List<ExamResult>> GetByExamIdAsync(int examId);
}
