using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUserNameAsync(string userName);
}
