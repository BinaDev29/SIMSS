// Application/Contracts/ICustomerRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
        Task GetCustomerWithDetailsAsync(int id, CancellationToken cancellationToken);
        Task<PagedResult<Customer>> GetPagedCustomersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}