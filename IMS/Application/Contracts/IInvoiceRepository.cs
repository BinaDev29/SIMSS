// Application/Contracts/IInvoiceRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage; // Add this using directive

namespace Application.Contracts
{
    // Assuming IGenericRepository doesn't have BeginTransactionAsync
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice?> GetLastInvoiceAsync(CancellationToken cancellationToken);

        // Add this method to the interface
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}