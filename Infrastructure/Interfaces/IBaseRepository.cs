using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity entity);
        bool Delete(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(int id, TEntity entity);
    }
}