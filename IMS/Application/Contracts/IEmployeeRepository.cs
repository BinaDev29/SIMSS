// Application/Contracts/IEmployeeRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<PagedResult<Employee>> GetPagedEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<Employee?> GetEmployeeWithDetailsAsync(int id, CancellationToken cancellationToken);
    }
}