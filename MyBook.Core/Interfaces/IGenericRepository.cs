using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task CreateAll(List<TEntity> items);
        Task Create(TEntity item);
        Task<TEntity> FindById(int id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        Task Remove(TEntity item);
        Task RemoveAll(List<TEntity> items);
        Task RemoveById(int id);
        Task Update(TEntity item, List<string>? excluded);
        IQueryable<TEntity> GetWithInclude(
            params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TResult> GetWithMultiIncluding<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);
    }
}