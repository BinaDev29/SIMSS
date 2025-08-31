// Persistence/Repositories/DeliveryDetailRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeliveryDetailRepository(SIMSDbContext context) : GenericRepository<DeliveryDetail>(context), IDeliveryDetailRepository
    {
        private new readonly SIMSDbContext _context = context;

        public async Task<DeliveryDetail?> GetDeliveryDetailWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails
                .Include(x => x.Delivery)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<DeliveryDetail>> GetByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken)
        {
            return await _context.DeliveryDetails
                .Where(x => x.DeliveryId == deliveryId)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        Task<DeliveryDetail?> IDeliveryDetailRepository.GetDeliveryDetailWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<DeliveryDetail>> IDeliveryDetailRepository.GetByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IDeliveryDetailRepository.AddAsync(DeliveryDetail deliveryDetail, object cancellationationToken)
        {
            throw new NotImplementedException();
        }
    }
}