// Persistence/Repositories/UserRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository(SIMSDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
    {
        private new readonly SIMSDbContext _context = dbContext;

        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<PagedResult<User>> GetPagedUsersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<User>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.Username.Contains(searchTerm) );
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<User>(items, totalCount, pageNumber, pageSize);
        }
    }
}