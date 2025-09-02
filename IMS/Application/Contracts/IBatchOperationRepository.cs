// Application/Contracts/IBatchOperationRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IBatchOperationRepository : IGenericRepository<BatchOperation>
    {
        Task<IReadOnlyList<BatchOperation>> GetOperationsByTypeAsync(string operationType, CancellationToken cancellationToken);
        Task<IReadOnlyList<BatchOperation>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken);
        Task<IReadOnlyList<BatchOperation>> GetOperationsByUserAsync(string userId, CancellationToken cancellationToken);
        Task<PagedResult<BatchOperation>> GetPagedOperationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}