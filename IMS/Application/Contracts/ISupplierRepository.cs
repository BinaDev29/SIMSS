// Application/Contracts/ISupplierRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<Supplier?> GetSupplierByNameAsync(string name, CancellationToken cancellationToken);
        Task<bool> HasTransactionsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken);
        Task<PagedResult<Supplier>> GetPagedSuppliersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}