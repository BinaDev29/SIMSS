// Persistence/Repositories/EmployeeRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly SIMSDbContext _context;

        public EmployeeRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Employee?> GetEmployeeByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        }

        public async Task<PagedResult<Employee>> GetPagedEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Employee>()
                .Include(e => e.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.FirstName.Contains(searchTerm) || 
                                        e.LastName.Contains(searchTerm) || 
                                        e.Email!.Contains(searchTerm) ||
                                        e.JobTitle!.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Employee>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<Employee>> GetActiveEmployeesAsync(CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .Include(e => e.User)
                .ToListAsync(cancellationToken);
        }

        public Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasTransactionsByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Application.DTOs.Common.PagedResult<Employee>> IEmployeeRepository.GetPagedEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> GetEmployeeWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}