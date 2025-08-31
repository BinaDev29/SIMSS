// Persistence/Repositories/InwardTransactionRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InwardTransactionRepository(SIMSDbContext dbContext) : GenericRepository<InwardTransaction>(dbContext), IInwardTransactionRepository
    {
        private new readonly SIMSDbContext _context = dbContext;

        public async Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions.AnyAsync(t => t.ItemId == itemId, cancellationToken);
        }

        public async Task<bool> HasTransactionsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions.AnyAsync(t => t.SupplierId == supplierId, cancellationToken);
        }

        public async Task<PagedResult<InwardTransaction>> GetPagedInwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InwardTransaction>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Include(it => it.Item)
                             .Include(it => it.Supplier)
                             .Where(it => it.Item!.ItemName.Contains(searchTerm));
            }
            else
            {
                query = query.Include(it => it.Item)
                             .Include(it => it.Supplier);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<InwardTransaction>(items, totalCount, pageNumber, pageSize);
        }
    }
}