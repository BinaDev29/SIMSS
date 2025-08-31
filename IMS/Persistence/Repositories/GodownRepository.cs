// Persistence/Repositories/GodownRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GodownRepository(SIMSDbContext dbContext) : GenericRepository<Godown>(dbContext), IGodownRepository
    {
        private new readonly SIMSDbContext _context = dbContext;

        public async Task<Godown?> GetGodownByNameOrCodeAsync(string name, string code, CancellationToken cancellationToken)
        {
            return await _context.Godowns.FirstOrDefaultAsync(g => g.GodownName == name , cancellationToken);
        }

        public async Task<PagedResult<Godown>> GetPagedGodownsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Godown>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.GodownName.Contains(searchTerm)  || g.Location.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Godown>(items, totalCount, pageNumber, pageSize);
        }
    }
}