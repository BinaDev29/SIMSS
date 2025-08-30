// Persistence/Repositories/InvoiceDetailRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceDetailRepository(SIMSDbContext dbContext) : GenericRepository<InvoiceDetail>(dbContext), IInvoiceDetailRepository
    {
        public async Task<List<InvoiceDetail>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken)
        {
            return await DbContext.InvoiceDetails
                .Where(id => id.InvoiceId == invoiceId)
                .ToListAsync(cancellationToken);
        }

        public Task DeleteRangeAsync(ICollection<InvoiceDetail> invoiceDetails, CancellationToken cancellationToken)
        {
            DbContext.InvoiceDetails.RemoveRange(invoiceDetails);
            return Task.CompletedTask;
        }
    }
}