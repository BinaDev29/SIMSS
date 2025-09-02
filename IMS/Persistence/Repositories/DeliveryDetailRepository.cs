// Persistence/Repositories/DeliveryDetailRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeliveryDetailRepository : GenericRepository<DeliveryDetail>, IDeliveryDetailRepository
    {
        private readonly SIMSDbContext _context;

        public DeliveryDetailRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<DeliveryDetail>> GetDetailsByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails
                .Where(dd => dd.DeliveryId == deliveryId)
                .Include(dd => dd.Item)
                .Include(dd => dd.Godown)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasDetailsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails.AnyAsync(dd => dd.ItemId == itemId, cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<DeliveryDetail> entities, CancellationToken cancellationToken)
        {
            _context.DeliveryDetails.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<DeliveryDetail>> GetPagedDeliveryDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.DeliveryDetails.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Include(dd => dd.Item)
                             .Include(dd => dd.Godown)
                             .Where(dd => dd.Item!.ItemName.Contains(searchTerm) || dd.Godown!.GodownName.Contains(searchTerm));
            }
            else
            {
                query = query.Include(dd => dd.Item)
                             .Include(dd => dd.Godown);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<DeliveryDetail>(items, totalCount, pageNumber, pageSize);
        }
    }
}
