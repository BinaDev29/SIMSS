// Persistence/Repositories/CustomerRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly SIMSDbContext _context;

        public CustomerRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Customer?> GetCustomerByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerName == name, cancellationToken);
        }

        public async Task<bool> HasTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions.AnyAsync(t => t.CustomerId == customerId, cancellationToken);
        }

        public async Task<PagedResult<Customer>> GetPagedCustomersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Customer>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.CustomerName.Contains(searchTerm) || 
                                        c.ContactPerson.Contains(searchTerm) || 
                                        c.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Customer>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<Customer>> ICustomerRepository.GetPagedCustomersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}