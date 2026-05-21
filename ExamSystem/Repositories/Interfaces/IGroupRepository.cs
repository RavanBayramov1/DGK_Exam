using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Interfaces;

public interface IGroupRepository : IGenericRepository<Group>
{
    Task<Group?> GetWithDetailsAsync(int id); // Students, Teachers ilə birlikdə
    Task<List<Group>> GetByTeacherIdAsync(int teacherId);
}
