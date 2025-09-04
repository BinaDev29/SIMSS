// Persistence/Repositories/InvoiceRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly SIMSDbContext _context;

        public InvoiceRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Invoice?> GetInvoiceByNumberAsync(string invoiceNumber, CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.InvoiceDetails)
                    .ThenInclude(id => id.Item)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
        }

        public async Task<IReadOnlyList<Invoice>> GetInvoicesByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Where(i => i.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Invoice>> GetUnpaidInvoicesAsync(CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Where(i => !i.IsPaid)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<Invoice>> GetPagedInvoicesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Invoice>()
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(i => i.InvoiceNumber.Contains(searchTerm) || 
                                        i.Customer!.CustomerName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Invoice>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<Invoice>> IInvoiceRepository.GetPagedInvoicesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}