using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyBook.Entities;
using Repositories;

namespace MyBook.Infrastructure.Repositories
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly MyBookContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public EfGenericRepository(MyBookContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return DbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate).ToList();
        }

        public async Task<TEntity> FindById(int id) => await DbSet.FindAsync(id);

        public async Task Create(TEntity item)
        {
            Context.Entry(item).State = EntityState.Added;
            await DbSet.AddAsync(item);
            await Context.SaveChangesAsync();
        }
            
        public async Task Update(TEntity item, List<string>? excluded = null)
        {
            var entity = Context.Entry(item);
            entity.State = EntityState.Modified;

            if (excluded != null)
                foreach (var name in excluded)
                {
                    entity.Property(name).IsModified = false;
                }

            await Context.SaveChangesAsync();
        }

        public async Task Remove(TEntity item)
        {
            DbSet.Remove(item);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveById(int id)
        {
            var entity = await FindById(id);
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetWithInclude(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties);
        }

        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.AsEnumerable().Where(predicate).ToList();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = DbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TResult> GetWithMultiIncluding<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.Select(selector);
        }

        public Task CreateAll(List<TEntity> items)
        {
            Context.AddRange(items);
            return Context.SaveChangesAsync();
        }

        public async Task RemoveAll(List<TEntity> items)
        {
            DbSet.RemoveRange(items);
            await Context.SaveChangesAsync();
        }
    }
}