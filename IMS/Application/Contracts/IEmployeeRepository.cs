// Application/Contracts/IEmployeeRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IReadOnlyList<Employee>> GetAllEmployeesWithUsersAsync(CancellationToken cancellationToken);
        Task<Employee?> GetEmployeeWithUserByIdAsync(int id, CancellationToken cancellationToken);
    }
}