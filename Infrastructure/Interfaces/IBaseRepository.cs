using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Create(TEntity entity);
        Task<bool> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Update(int id, TEntity entity);
    }
}