// Persistence/Repositories/DeliveryRepository.cs
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
    public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
    {
        private readonly SIMSDbContext _context;

        public DeliveryRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Delivery?> GetDeliveryWithDetailsAsync(int deliveryId, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .Include(d => d.DeliveryDetails)
                    .ThenInclude(dd => dd.Item)
                .Include(d => d.DeliveryDetails)
                    .ThenInclude(dd => dd.Godown)
                .FirstOrDefaultAsync(d => d.Id == deliveryId, cancellationToken);
        }

        public async Task<bool> HasDeliveriesByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Deliveries.AnyAsync(d => d.CustomerId == customerId, cancellationToken);
        }

        public async Task<PagedResult<Delivery>> GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Deliveries.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Include(d => d.Customer)
                             .Where(d => d.Customer!.CustomerName.Contains(searchTerm) || d.DeliveryNumber.Contains(searchTerm));
            }
            else
            {
                query = query.Include(d => d.Customer);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(d => d.DeliveryDate)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);

            return new PagedResult<Delivery>(items, totalCount, pageNumber, pageSize);
        }
    }
}
