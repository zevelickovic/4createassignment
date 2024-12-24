using System.Linq.Expressions;

namespace Assignment.Application.Interfaces;

public interface IRepository<TEntity, in TId> where TEntity : class
{
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(TId id);
    void Insert(TEntity entity);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> SaveChangesAsync();
}

public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class
{

}