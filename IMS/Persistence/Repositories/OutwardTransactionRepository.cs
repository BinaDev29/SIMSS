// Persistence/Repositories/OutwardTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OutwardTransactionRepository(SIMSDbContext context) : GenericRepository<OutwardTransaction>(context), IOutwardTransactionRepository
    {
        private new readonly SIMSDbContext _context = context;

        public async Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Customer)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OutwardTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Where(x => x.OutwardDate >= startDate && x.OutwardDate <= endDate)
                .Include(x => x.Customer)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions.AnyAsync(x => x.CustomerId == customerId, cancellationToken);
        }

        public async Task<OutwardTransaction?> GetTransactionWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions
                .Include(x => x.Customer)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<bool> HasTransactionsByCustomerId(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetPagedOutwardTransactionsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasTransactionsByItemIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasTransactionsByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}