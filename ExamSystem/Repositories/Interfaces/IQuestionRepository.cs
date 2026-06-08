using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<Question?> GetByIdAsync(int id);
    Task<List<Question>> GetByTeacherIdAsync(int teacherId);
    Task<List<Question>> GetBySubjectIdAsync(int subjectId);
    Task<List<Question>> GetByExamIdAsync(int examId);
    Task<List<Question>> GetAllWithDetailAsync();

    Task<Question?> GetByIdWithDetailAsync(int id);
}
