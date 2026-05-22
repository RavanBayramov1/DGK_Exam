using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Interfaces;

public interface IResultRepository : IGenericRepository<ExamResult>
{
    Task<ExamResult?> GetByExamAndStudentAsync(int examId, int studentId);
    Task<List<ExamResult>> GetByStudentIdAsync(int studentId);
    Task<List<ExamResult>> GetByExamIdAsync(int examId);
    Task<List<ExamResult>> GetActiveResultsAsync();
    Task<List<ExamResult>> GetAllWithDetailsAsync();
    Task<ExamResult?> GetByIdWithDetailsAsync(int id);
}
