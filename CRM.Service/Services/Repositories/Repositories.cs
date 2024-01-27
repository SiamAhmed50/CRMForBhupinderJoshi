using CRM.Data.DbContext;
using CRM.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Services.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly ProjectDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;

        public Repository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        // Rest of the implementation...

        // Expose _dbSet as a protected property or method
        protected DbSet<TEntity> GetDbSet()
        {
            return _dbSet;
        }

        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            // Include navigation properties
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Get the type of the entity
            var entityType = typeof(TEntity);

            // Get the property named "Id" from the entity type
            var idProperty = entityType.GetProperty("Id");

            // Check if the entity has an "Id" property
            if (idProperty != null)
            {
                // Build the lambda expression for the query
                 var parameter = Expression.Parameter(entityType, "entity");
                var propertyAccess = Expression.Property(parameter, idProperty);
                var equality = Expression.Equal(propertyAccess, Expression.Constant(id));
                var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                // Apply the filter to the query
                query = query.Where(lambda);
            }

            return await query.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            // Include related entities
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Apply filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }


        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

      
    }
}
