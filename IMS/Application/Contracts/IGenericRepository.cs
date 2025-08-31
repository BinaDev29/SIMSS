// Application/Contracts/IGenericRepository.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task Update(T entity, CancellationToken cancellationToken);
        Task Delete(T entity, CancellationToken cancellationToken);
    }
}