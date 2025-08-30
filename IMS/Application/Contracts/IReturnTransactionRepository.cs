// Application/Contracts/IReturnTransactionRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage; // Add this using directive

namespace Application.Contracts
{
    // Assuming IGenericRepository doesn't have BeginTransactionAsync
    public interface IReturnTransactionRepository : IGenericRepository<ReturnTransaction>
    {
        // Your other methods...

        // Add this method to the interface
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}