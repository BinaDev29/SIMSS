// Persistence/Repositories/GodownRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GodownRepository : GenericRepository<Godown>, IGodownRepository
    {
        private readonly SIMSDbContext _context;

        public GodownRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Godown?> GetGodownByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Godowns.FirstOrDefaultAsync(g => g.GodownName == name, cancellationToken);
        }

        public async Task<bool> HasInventoryByGodownIdAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.GodownInventories.AnyAsync(gi => gi.GodownId == godownId, cancellationToken);
        }

        public async Task<PagedResult<Godown>> GetPagedGodownsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Godown>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.GodownName.Contains(searchTerm) || 
                                        g.Location.Contains(searchTerm) ||
                                        g.GodownManager!.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Godown>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<Godown>> IGodownRepository.GetPagedGodownsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}