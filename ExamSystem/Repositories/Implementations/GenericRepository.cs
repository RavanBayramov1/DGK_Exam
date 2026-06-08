using ExamSystem.Data;
using ExamSystem.Models.Common;
using ExamSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExamSystem.Repositories.Implementations;

public class GenericRepository<T>(AppDbContext _context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet = _context.Set<T>();

    public async Task<List<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) =>
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

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}
