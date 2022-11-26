using System.Linq.Expressions;
using Checkout.Custom.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Custom.Common.Repository
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly DbContext _dataContext;
        public Repository(DbContext bankContext)
        {
            this._dataContext = bankContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dataContext.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return _dataContext.Set<T>().Where(where).AsQueryable();
        }
    }
}
