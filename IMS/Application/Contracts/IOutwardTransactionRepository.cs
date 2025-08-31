// Application/Contracts/IOutwardTransactionRepository.cs
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IOutwardTransactionRepository : IGenericRepository<OutwardTransaction>
    {
        Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<OutwardTransaction?> GetTransactionWithDetailsAsync(int id, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByCustomerId(int id);
        Task GetPagedOutwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByItemIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByEmployeeIdAsync(int id, CancellationToken cancellationToken);
    }
}