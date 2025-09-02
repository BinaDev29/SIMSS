// Persistence/Repositories/ReturnTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ReturnTransactionRepository : GenericRepository<ReturnTransaction>, IReturnTransactionRepository
    {
        private readonly SIMSDbContext _context;

        public ReturnTransactionRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.ReturnTransactions
                .Include(rt => rt.Item)
                .Include(rt => rt.Customer)
                .Include(rt => rt.Employee)
                .Where(rt => rt.GodownId == godownId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.ReturnTransactions
                .Include(rt => rt.Godown)
                .Include(rt => rt.Customer)
                .Include(rt => rt.Employee)
                .Where(rt => rt.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.ReturnTransactions
                .Include(rt => rt.Item)
                .Include(rt => rt.Godown)
                .Include(rt => rt.Employee)
                .Where(rt => rt.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<ReturnTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<ReturnTransaction>()
                .Include(rt => rt.Item)
                .Include(rt => rt.Godown)
                .Include(rt => rt.Customer)
                .Include(rt => rt.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(rt => rt.Item!.ItemName.Contains(searchTerm) || 
                                         rt.Customer!.CustomerName.Contains(searchTerm) ||
                                         rt.Reason.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ReturnTransaction>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<ReturnTransaction>> IReturnTransactionRepository.GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}