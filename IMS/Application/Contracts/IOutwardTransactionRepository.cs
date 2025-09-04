// Application/Contracts/IOutwardTransactionRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IOutwardTransactionRepository : IGenericRepository<OutwardTransaction>
    {
        Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<PagedResult<OutwardTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<PagedResult<OutwardTransaction>> GetPagedOutwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByCustomerId(int id);
        Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken);
    }
}