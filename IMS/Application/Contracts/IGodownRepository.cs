// Application/Contracts/IGodownRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IGodownRepository : IGenericRepository<Godown>
    {
        Task<Godown?> GetGodownByNameOrCodeAsync(string name, string code, CancellationToken cancellationToken);
        Task<PagedResult<Godown>> GetPagedGodownsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}