// Persistence/Repositories/DeliveryRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Delivery?> GetDeliveryByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .Include(d => d.Customer)
                .Include(d => d.DeliveryDetails)
                    .ThenInclude(dd => dd.Item)
                .FirstOrDefaultAsync(d => d.TrackingNumber == trackingNumber, cancellationToken);
        }

        public async Task<IReadOnlyList<Delivery>> GetDeliveriesByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .Include(d => d.Customer)
                .Where(d => d.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Delivery>> GetDeliveriesByStatusAsync(string status, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .Include(d => d.Customer)
                .Where(d => d.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<Delivery>> GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Delivery>()
                .Include(d => d.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.TrackingNumber.Contains(searchTerm) || 
                                        d.Customer!.CustomerName.Contains(searchTerm) ||
                                        d.Status.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Delivery>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<Delivery>> IDeliveryRepository.GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}