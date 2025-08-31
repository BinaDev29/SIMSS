// Persistence/Repositories/InvoiceRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceRepository(SIMSDbContext context) : GenericRepository<Invoice>(context), IInvoiceRepository
    {
        private new readonly SIMSDbContext _context = context;

        public async Task<Application.Contracts.IDbContextTransaction> BeginTransactionAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new DbContextTransactionWrapper(transaction);
        }

        public async Task<Invoice?> GetInvoiceWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .Include(x => x.Customer)
                .Include(x => x.InvoiceDetails)
                    .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task GetPagedInvoicesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Application.Contracts.IDbContextTransaction> IInvoiceRepository.BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class DbContextTransactionWrapper : Application.Contracts.IDbContextTransaction
    {
        private readonly Application.Contracts.IDbContextTransaction _transaction;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction;

        public DbContextTransactionWrapper(Application.Contracts.IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public DbContextTransactionWrapper(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}