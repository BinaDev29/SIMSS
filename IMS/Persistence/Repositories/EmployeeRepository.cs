// Persistence/Repositories/EmployeeRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EmployeeRepository(SIMSDbContext dbContext) : GenericRepository<Employee>(dbContext), IEmployeeRepository
    {
        public async Task<IReadOnlyList<Employee>> GetAllEmployeesWithUsersAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Employees.Include(e => e.User).ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetEmployeeWithUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbContext.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}