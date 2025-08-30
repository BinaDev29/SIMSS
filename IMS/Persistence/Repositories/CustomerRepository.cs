// Persistence/Repositories/CustomerRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CustomerRepository(SIMSDbContext context) : GenericRepository<Customer>(context), ICustomerRepository
    {
        public async Task<IEnumerable<Customer>> GetAllWithInvoicesAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Customers
                .Include(c => c.Invoices)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdWithInvoicesAsync(int id, CancellationToken cancellationToken)
        {
            return await DbContext.Customers
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}