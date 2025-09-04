// Persistence/Repositories/OutwardTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OutwardTransactionRepository : GenericRepository<OutwardTransaction>, IOutwardTransactionRepository
    {
        private readonly SIMSDbContext _context;

        public OutwardTransactionRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Include(ot => ot.Item)
                .Include(ot => ot.Customer)
                .Include(ot => ot.Employee)
                .Where(ot => ot.GodownId == godownId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Include(ot => ot.Godown)
                .Include(ot => ot.Customer)
                .Include(ot => ot.Employee)
                .Where(ot => ot.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Include(ot => ot.Item)
                .Include(ot => ot.Godown)
                .Include(ot => ot.Employee)
                .Where(ot => ot.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<OutwardTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<OutwardTransaction>()
                .Include(ot => ot.Item)
                .Include(ot => ot.Godown)
                .Include(ot => ot.Customer)
                .Include(ot => ot.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ot => ot.Item!.ItemName.Contains(searchTerm) || 
                                         ot.Customer!.CustomerName.Contains(searchTerm) ||
                                         ot.InvoiceNumber.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<OutwardTransaction>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<OutwardTransaction>> IOutwardTransactionRepository.GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Application.DTOs.Common.PagedResult<OutwardTransaction>> GetPagedOutwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasTransactionsByCustomerId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}