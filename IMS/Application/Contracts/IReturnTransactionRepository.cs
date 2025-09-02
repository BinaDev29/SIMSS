// Application/Contracts/IReturnTransactionRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IReturnTransactionRepository : IGenericRepository<ReturnTransaction>
    {
        Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<PagedResult<ReturnTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}