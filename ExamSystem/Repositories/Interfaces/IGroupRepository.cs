using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IGroupRepository : IGenericRepository<Group>
{
    Task<Group?> GetWithDetailsAsync(int id); // Students, Teachers ilə birlikdə
    Task<List<Group>> GetByTeacherIdAsync(int teacherId);
}
