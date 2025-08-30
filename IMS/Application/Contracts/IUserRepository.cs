// Application/Contracts/IUserRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
    }
}