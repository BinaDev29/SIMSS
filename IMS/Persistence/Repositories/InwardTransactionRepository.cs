// Persistence/Repositories/InwardTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InwardTransactionRepository : GenericRepository<InwardTransaction>, IInwardTransactionRepository
    {
        private readonly SIMSDbContext _context;

        public InwardTransactionRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<InwardTransaction>> GetTransactionsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions
                .Include(it => it.Item)
                .Include(it => it.Supplier)
                .Where(it => it.GodownId == godownId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InwardTransaction>> GetTransactionsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions
                .Include(it => it.Godown)
                .Include(it => it.Supplier)
                .Where(it => it.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InwardTransaction>> GetTransactionsBySupplierAsync(int supplierId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions
                .Include(it => it.Item)
                .Include(it => it.Godown)
                .Where(it => it.SupplierId == supplierId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<InwardTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InwardTransaction>()
                .Include(it => it.Item)
                .Include(it => it.Godown)
                .Include(it => it.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(it => it.Item!.ItemName.Contains(searchTerm) || 
                                         it.Supplier!.SupplierName.Contains(searchTerm) ||
                                         it.InvoiceNumber!.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InwardTransaction>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<InwardTransaction>> IInwardTransactionRepository.GetPagedTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}