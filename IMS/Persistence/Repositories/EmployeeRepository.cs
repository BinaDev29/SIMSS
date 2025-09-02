// Persistence/Repositories/EmployeeRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EmployeeRepository(SIMSDbContext dbContext) : GenericRepository<Employee>(dbContext), IEmployeeRepository
    {
        public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
        }

        public async Task<bool> HasTransactionsByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            // Assuming employee is related to all transaction types
            return await _dbContext.InwardTransactions.AnyAsync(t => t.EmployeeId == employeeId, cancellationToken) ||
                   await _dbContext.OutwardTransactions.AnyAsync(t => t.EmployeeId == employeeId, cancellationToken) ||
                   await _dbContext.ReturnTransactions.AnyAsync(t => t.EmployeeId == employeeId, cancellationToken);
        }

        public async Task<PagedResult<Employee>> GetPagedEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<Employee>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.FirstName.Contains(searchTerm) || e.LastName.Contains(searchTerm) || e.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(e => e.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Employee>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}