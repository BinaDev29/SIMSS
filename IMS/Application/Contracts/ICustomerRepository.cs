// Application/Contracts/ICustomerRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllWithInvoicesAsync(CancellationToken cancellationToken);
        Task<Customer?> GetByIdWithInvoicesAsync(int id, CancellationToken cancellationToken);
    }
}