using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IBaseRepository<T>
    {
        Task CreateAsync(CancellationToken cancellationToken, T entity);
        Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] keys);
        void Update(CancellationToken cancellationToken, T entity);
    }
}
