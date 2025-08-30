// Persistence/Repositories/GenericRepository.cs
using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SIMSDbContext DbContext;

        public GenericRepository(SIMSDbContext context)
        {
            DbContext = context;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        // Add and save the entity, returning the added entity.
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await DbContext.Set<T>().AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}