// Application/Contracts/IBatchOperationRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IBatchOperationRepository : IGenericRepository<BatchOperation>
    {
        Task<PagedResult<BatchOperation>> GetPagedBatchOperationsAsync(int pageNumber, int pageSize, string? operationType, string? status, CancellationToken cancellationToken);
        Task<IReadOnlyList<BatchOperation>> GetBatchOperationsByStatusAsync(string status, CancellationToken cancellationToken);
        Task<IReadOnlyList<BatchOperation>> GetBatchOperationsByUserAsync(string userId, CancellationToken cancellationToken);
        Task UpdateProgressAsync(int batchOperationId, int processedRecords, int successfulRecords, int failedRecords, CancellationToken cancellationToken);
        Task UpdateStatusAsync(int batchOperationId, string status, CancellationToken cancellationToken);
    }
}