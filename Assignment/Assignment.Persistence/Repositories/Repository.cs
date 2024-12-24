using Assignment.Application.Interfaces;
using Assignment.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Assignment.Persistence.Repositories;

public class Repository<TEntity>(DatabaseContext dbContext) : IRepository<TEntity> where TEntity : class, IEntity
{
    internal DatabaseContext DbContext = dbContext;
    internal DbSet<TEntity> DbSet = dbContext.Set<TEntity>();
    public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Insert(TEntity entity)
    {
        DbContext.Add(entity);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync();
    }
}