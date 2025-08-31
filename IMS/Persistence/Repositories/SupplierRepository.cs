// Persistence/Repositories/SupplierRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SupplierRepository(SIMSDbContext dbContext) : GenericRepository<Supplier>(dbContext), ISupplierRepository
    {
        private new readonly SIMSDbContext _context = dbContext;

        public async Task<Supplier?> GetSupplierByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName == name, cancellationToken);
        }

        public async Task<bool> HasTransactionsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken)
        {
            return await _context.InwardTransactions.AnyAsync(t => t.SupplierId == supplierId, cancellationToken);
        }

        public async Task<PagedResult<Supplier>> GetPagedSuppliersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Supplier>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.SupplierName.Contains(searchTerm) || s.ContactPerson.Contains(searchTerm) || s.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Supplier>(items, totalCount, pageNumber, pageSize);
        }
    }
}