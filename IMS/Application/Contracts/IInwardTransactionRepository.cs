// Application/Contracts/IInwardTransactionRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInwardTransactionRepository : IGenericRepository<InwardTransaction>
    {
        Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<bool> HasTransactionsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken);
        Task<PagedResult<InwardTransaction>> GetPagedInwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}