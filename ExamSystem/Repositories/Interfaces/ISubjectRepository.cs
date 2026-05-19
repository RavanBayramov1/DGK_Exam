using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface ISubjectRepository : IGenericRepository<Subject>
{
    Task<List<Subject>> GetByGroupIdAsync(int groupId);
}
