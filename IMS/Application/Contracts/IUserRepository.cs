// Application/Contracts/IUserRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
        Task<PagedResult<User>> GetPagedUsersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}