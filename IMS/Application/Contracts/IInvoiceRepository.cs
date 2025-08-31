// Application/Contracts/IInvoiceRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Invoice?> GetInvoiceWithDetailsAsync(int id, CancellationToken cancellationToken);
        Task GetPagedInvoicesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}