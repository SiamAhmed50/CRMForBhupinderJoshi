using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                     params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
    }

   

}
