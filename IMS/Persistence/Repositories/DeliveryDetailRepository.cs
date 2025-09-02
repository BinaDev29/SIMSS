// Persistence/Repositories/DeliveryDetailRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<DeliveryDetail>> GetDetailsByDeliveryAsync(int deliveryId, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails
                .Include(dd => dd.Item)
                .Include(dd => dd.Godown)
                .Where(dd => dd.DeliveryId == deliveryId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DeliveryDetail>> GetDetailsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails
                .Include(dd => dd.Delivery)
                    .ThenInclude(d => d!.Customer)
                .Where(dd => dd.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<DeliveryDetail>> GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<DeliveryDetail>()
                .Include(dd => dd.Delivery)
                .Include(dd => dd.Item)
                .Include(dd => dd.Godown)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(dd => dd.Item!.ItemName.Contains(searchTerm) || 
                                         dd.Delivery!.TrackingNumber.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<DeliveryDetail>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<DeliveryDetail>> IDeliveryDetailRepository.GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}