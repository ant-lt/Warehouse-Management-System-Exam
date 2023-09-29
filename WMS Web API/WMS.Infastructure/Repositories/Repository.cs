using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly WMSContext _db;
        private DbSet<TEntity> _dbSet;

        public Repository(WMSContext db)
        { 
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            if (await SaveAsync() > 0) 
                return true;
            else 
                return false;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracked = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter, ICollection<string> includeTables)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            foreach (var tableName in includeTables)
            {
                query = query.Include(tableName);
            }
            return await query.ToListAsync();
        }


        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, ICollection<string> includeTables, bool tracked = true)
        {

            IQueryable<TEntity> query = _dbSet;
            if (!tracked) query = query.AsNoTracking();
            query = query.Where(filter);
            foreach (var tableName in includeTables)
            {
                query = query.Include(tableName);
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }
    }
}
