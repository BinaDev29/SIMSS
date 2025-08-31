// Persistence/Repositories/GenericRepository.cs
using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SIMSDbContext _context;

        public GenericRepository(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public void Update(T entity, CancellationToken cancellationToken)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Remove(entity);
        }

        Task IGenericRepository<T>.Update(T entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<T>.Delete(T entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}