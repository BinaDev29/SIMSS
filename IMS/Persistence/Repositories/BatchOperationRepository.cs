// Persistence/Repositories/BatchOperationRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BatchOperationRepository : GenericRepository<BatchOperation>, IBatchOperationRepository
    {
        private readonly SIMSDbContext _context;

        public BatchOperationRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<BatchOperation>> GetAllBatchOperationsAsync(CancellationToken cancellationToken)
        {
            return await _context.BatchOperations
                .OrderByDescending(b => b.StartedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<BatchOperation?> GetBatchOperationByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.BatchOperations.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddBatchOperationAsync(BatchOperation batchOperation, CancellationToken cancellationToken)
        {
            await _context.BatchOperations.AddAsync(batchOperation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBatchOperationAsync(BatchOperation batchOperation, CancellationToken cancellationToken)
        {
            _context.Entry(batchOperation).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBatchOperationAsync(BatchOperation batchOperation, CancellationToken cancellationToken)
        {
            _context.BatchOperations.Remove(batchOperation);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
