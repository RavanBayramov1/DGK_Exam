using ExamSystem.Models.Common;

namespace ExamSystem.Repositories.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    Task<bool> SaveChangesAsync();
}
