// Application/Contracts/IReturnTransactionRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IReturnTransactionRepository : IGenericRepository<ReturnTransaction>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task GetPagedReturnTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<IReadOnlyList<ReturnTransaction>> GetReturnsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<ReturnTransaction?> GetReturnWithDetailsAsync(int id, CancellationToken cancellationToken);
    }
}