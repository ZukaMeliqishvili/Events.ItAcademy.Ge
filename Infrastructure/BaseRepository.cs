using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Persistance.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> dbset;

        public BaseRepository(ApplicationDbContext db)
        {
            this.db = db;
            this.dbset = db.Set<T>();
        }

        public async Task CreateAsync(CancellationToken cancellationToken, T entity)
        {
            await db.AddAsync<T>(entity, cancellationToken);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            int n = await query.CountAsync<T>(filter, cancellationToken);
            return n > 0;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            IQueryable<T> query = dbset;
            await query.LoadAsync(cancellationToken);
            return query.ToList();
        }

        public async Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] keys)
        {
            return await dbset.FindAsync(keys, cancellationToken);

        }
        public async Task DeleteAsync(CancellationToken cancellationToken, params object[] key)
        {
            T obj = await dbset.FindAsync(key, cancellationToken);
            dbset.Remove(obj);
        }
        public void Update(CancellationToken cancellationToken, T entity)
        {
            if (entity == null)
                return;
            this.dbset.Update(entity);
        }
    }
}
