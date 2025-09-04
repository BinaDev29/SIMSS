// Application/Contracts/IInwardTransactionRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInwardTransactionRepository : IGenericRepository<InwardTransaction>
    {
        Task<IReadOnlyList<InwardTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InwardTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InwardTransaction>> GetTransactionsBySupplierAsync(int supplierId, CancellationToken cancellationToken);
        Task<PagedResult<InwardTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<PagedResult<InwardTransaction>> GetPagedInwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<bool> HasTransactionsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken);
    }
}