// Persistence/Repositories/InvoiceDetailRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly SIMSDbContext _context;

        public InvoiceDetailRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<InvoiceDetail>> GetDetailsByInvoiceAsync(int invoiceId, CancellationToken cancellationToken)
        {
            return await _context.InvoiceDetails
                .Include(id => id.Item)
                .Include(id => id.Godown)
                .Where(id => id.InvoiceId == invoiceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InvoiceDetail>> GetDetailsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InvoiceDetails
                .Include(id => id.Invoice)
                    .ThenInclude(i => i!.Customer)
                .Where(id => id.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<InvoiceDetail>> GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InvoiceDetail>()
                .Include(id => id.Invoice)
                .Include(id => id.Item)
                .Include(id => id.Godown)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(id => id.Item!.ItemName.Contains(searchTerm) || 
                                         id.Invoice!.InvoiceNumber.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InvoiceDetail>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<InvoiceDetail>> IInvoiceDetailRepository.GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}