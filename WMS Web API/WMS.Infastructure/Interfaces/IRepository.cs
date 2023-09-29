using System.Linq.Expressions;

namespace WMS.Infastructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // CRUD
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter, ICollection<string> includeTables);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracked = true);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, ICollection<string> includeTables, bool tracked = true);
        Task<bool> CreateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<int> SaveAsync();
    }
}
