using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<List<Question>> GetByTeacherIdAsync(int teacherId);
    Task<List<Question>> GetBySubjectIdAsync(int subjectId);
    Task<List<Question>> GetByExamIdAsync(int examId);
}
