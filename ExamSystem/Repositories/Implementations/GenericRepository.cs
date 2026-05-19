using ExamSystem.Data;
using ExamSystem.Models.Common;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Repositories.Implemantations;

public class GenericRepository<T>(AddDbContext _context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet = _context.Set<T>();

    public async Task<List<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void SoftDelete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedTime = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;
}
