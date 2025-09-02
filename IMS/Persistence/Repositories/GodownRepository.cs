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
    public class GodownRepository : GenericRepository<Godown>, IGodownRepository
    {
        public GodownRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Godown?> GetGodownByNameOrCodeAsync(string name, string code, CancellationToken cancellationToken)
        {
            return await _dbContext.Godowns.FirstOrDefaultAsync(g => g.GodownName == name || g.GodownCode == code, cancellationToken);
        }

        public async Task<PagedResult<Godown>> GetPagedGodownsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<Godown>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.GodownName.Contains(searchTerm) || g.Location.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(g => g.GodownName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Godown>(items, totalCount, pageNumber, pageSize);
        }
    }
}
