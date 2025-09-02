// Persistence/Repositories/BatchOperationRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<BatchOperation>> GetOperationsByTypeAsync(string operationType, CancellationToken cancellationToken)
        {
            return await _context.BatchOperations
                .Where(bo => bo.OperationType == operationType)
                .OrderByDescending(bo => bo.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<BatchOperation>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken)
        {
            return await _context.BatchOperations
                .Where(bo => bo.Status == status)
                .OrderByDescending(bo => bo.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<BatchOperation>> GetOperationsByUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.BatchOperations
                .Where(bo => bo.InitiatedBy == userId)
                .OrderByDescending(bo => bo.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<BatchOperation>> GetPagedOperationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<BatchOperation>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(bo => bo.OperationType.Contains(searchTerm) || 
                                         bo.EntityType.Contains(searchTerm) ||
                                         bo.Status.Contains(searchTerm) ||
                                         bo.InitiatedBy.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(bo => bo.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<BatchOperation>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<BatchOperation>> IBatchOperationRepository.GetPagedOperationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}