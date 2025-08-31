// Persistence/Repositories/InvoiceDetailRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly SIMSDbContext _dbContext;

        public InvoiceDetailRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasDetailsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _dbContext.InvoiceDetails.AnyAsync(id => id.ItemId == itemId, cancellationToken);
        }

        public async Task<IReadOnlyList<InvoiceDetail>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken)
        {
            return await _dbContext.InvoiceDetails
                .Where(id => id.InvoiceId == invoiceId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<InvoiceDetail> entities, CancellationToken cancellationToken)
        {
            _dbContext.Set<InvoiceDetail>().RemoveRange(entities);
            await Task.CompletedTask; // Or use _dbContext.SaveChangesAsync() if you want to save changes
        }

        public async Task<PagedResult<InvoiceDetail>> GetPagedInvoiceDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<InvoiceDetail>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Include(id => id.Item)
                             .Include(id => id.Godown)
                             .Where(id => id.Item!.ItemName.Contains(searchTerm) || id.Godown!.GodownName.Contains(searchTerm));
            }
            else
            {
                query = query.Include(id => id.Item)
                             .Include(id => id.Godown);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<InvoiceDetail>(items, totalCount, pageNumber, pageSize);
        }
    }
}