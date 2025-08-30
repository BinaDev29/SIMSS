// Persistence/Repositories/InvoiceRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceRepository(SIMSDbContext dbContext) : GenericRepository<Invoice>(dbContext), IInvoiceRepository
    {
        public async Task<Invoice?> GetLastInvoiceAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Invoices
                .OrderByDescending(i => i.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // ይህን ዘዴ ለመጠቀም IGenericRepository ውስጥ መገለጽ አለበት።
        // This method should be defined in IGenericRepository to be accessible here.
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}