// Persistence/Repositories/DeliveryRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeliveryRepository(SIMSDbContext context) : GenericRepository<Delivery>(context), IDeliveryRepository
    {
        private new readonly SIMSDbContext _context = context;

        public async Task<Delivery?> GetDeliveryWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .Include(x => x.Customer)
                .Include(x => x.DeliveryDetails)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Delivery?> GetByOutwardTransactionIdAsync(int outwardTransactionId, CancellationToken cancellationToken)
        {
            return await _context.Deliveries
                .FirstOrDefaultAsync(x => x.OutwardTransactionId == outwardTransactionId, cancellationToken);
        }

        public async Task<PagedResult<Delivery>> GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Delivery>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.TrackingNumber.Contains(searchTerm) || d.Status.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Delivery>(items, totalCount, pageNumber, pageSize);
        }

        public Task GetByOutwardTransactionId(int outwardTransactionId)
        {
            throw new NotImplementedException();
        }
    }
}