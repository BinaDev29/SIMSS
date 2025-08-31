// Persistence/Repositories/ReturnTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ReturnTransactionRepository(SIMSDbContext context) : GenericRepository<ReturnTransaction>(context), IReturnTransactionRepository
    {
        private readonly SIMSDbContext _context = context;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new DbContextTransactionWrapper(transaction);
        }

        public Task GetPagedReturnTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ReturnTransaction>> GetReturnsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.ReturnTransactions
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Customer)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReturnTransaction?> GetReturnWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ReturnTransactions
                .Include(x => x.Customer)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}