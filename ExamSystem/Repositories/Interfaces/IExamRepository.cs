using ExamSystem.Enums;
using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Interfaces;

public interface IExamRepository : IGenericRepository<Exam>
{
    Task<Exam?> GetWithDetailsAsync(int id); // Questions, Group, Subject ilə
    Task<List<Exam>> GetByGroupIdAsync(int groupId);
    Task<List<Exam>> GetByTeacherIdAsync(int teacherId);
    Task<List<Exam>> GetActiveExamsAsync();
    Task<List<Exam>> GetAllWithDetailsAsync();
    Task<List<Exam>> GetActiveAndScheduledAsync();
}
