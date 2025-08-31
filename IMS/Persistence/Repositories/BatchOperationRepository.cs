// Persistence/Repositories/BatchOperationRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BatchOperationRepository : GenericRepository<BatchOperation>, IBatchOperationRepository
    {
        public BatchOperationRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<BatchOperation>> GetPagedBatchOperationsAsync(int pageNumber, int pageSize, string? operationType, string? status, CancellationToken cancellationToken)
        {
            var query = _context.Set<BatchOperation>().AsQueryable();

            if (!string.IsNullOrEmpty(operationType))
            {
                query = query.Where(b => b.OperationType == operationType);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(b => b.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<BatchOperation>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<BatchOperation>> GetBatchOperationsByStatusAsync(string status, CancellationToken cancellationToken)
        {
            return await _context.Set<BatchOperation>()
                .Where(b => b.Status == status)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<BatchOperation>> GetBatchOperationsByUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.Set<BatchOperation>()
                .Where(b => b.InitiatedBy == userId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateProgressAsync(int batchOperationId, int processedRecords, int successfulRecords, int failedRecords, CancellationToken cancellationToken)
        {
            var batchOperation = await _context.Set<BatchOperation>().FindAsync(new object[] { batchOperationId }, cancellationToken);
            if (batchOperation != null)
            {
                batchOperation.ProcessedRecords = processedRecords;
                batchOperation.SuccessfulRecords = successfulRecords;
                batchOperation.FailedRecords = failedRecords;
            }
        }

        public async Task UpdateStatusAsync(int batchOperationId, string status, CancellationToken cancellationToken)
        {
            var batchOperation = await _context.Set<BatchOperation>().FindAsync(new object[] { batchOperationId }, cancellationToken);
            if (batchOperation != null)
            {
                batchOperation.Status = status;
                if (status == "Processing" && batchOperation.StartedAt == null)
                {
                    batchOperation.StartedAt = DateTime.UtcNow;
                }
                else if (status == "Completed" || status == "Failed")
                {
                    batchOperation.CompletedAt = DateTime.UtcNow;
                }
            }
        }
    }
}