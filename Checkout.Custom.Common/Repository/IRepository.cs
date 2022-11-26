using System.Linq.Expressions;
using Checkout.Custom.Common.Entity;

namespace Checkout.Custom.Common.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetAsync(Guid id);
        Task<T> AddAsync(T entity);
        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }
}
