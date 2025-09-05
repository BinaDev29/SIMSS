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

        // ይህ የጎደለው method ነው
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
        Task<PagedResult<User>> GetPagedUsersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task GetUserByUsernameAsync(string? username);
        Task GetUserByEmailAsync(string? email);
    }
}