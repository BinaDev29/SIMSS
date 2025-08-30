// Persistence/Repositories/UserRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository(SIMSDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
    {
        public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}