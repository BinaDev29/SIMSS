// Persistence/Repositories/ReturnTransactionRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        // ... ??? methods ??? ??

        // ? method ????? ???? ????????
        public async Task<PagedResult<ReturnTransaction>> GetPagedReturnTransactionsAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            CancellationToken cancellationToken)
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

        public Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ReturnTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Application.DTOs.Common.PagedResult<ReturnTransaction>> IReturnTransactionRepository.GetPagedReturnTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}