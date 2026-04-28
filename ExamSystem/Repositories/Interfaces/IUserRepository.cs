using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByFullNameAsync(string fullName);
}
