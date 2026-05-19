using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IExamRepository : IGenericRepository<Exam>
{
    Task<Exam?> GetWithDetailsAsync(int id); // Questions, Group, Subject ilə
    Task<List<Exam>> GetByGroupIdAsync(int groupId);
    Task<List<Exam>> GetByTeacherIdAsync(int teacherId);
    Task<List<Exam>> GetActiveExamsAsync();
}
