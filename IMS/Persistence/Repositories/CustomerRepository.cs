// Persistence/Repositories/CustomerRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CustomerRepository(SIMSDbContext dbContext) : GenericRepository<Customer>(dbContext), ICustomerRepository
    {
        public async Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }

        public Task GetCustomerWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<Customer>> GetPagedCustomersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Customer>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm) || c.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Customer>(items, totalCount, pageNumber, pageSize);
        }
    }
}