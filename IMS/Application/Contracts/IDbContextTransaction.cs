// Application/Contracts/IDbContextTransaction.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDbContextTransaction : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}