// Persistence/Repositories/DeliveryDetailRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeliveryDetailRepository(SIMSDbContext context) : GenericRepository<DeliveryDetail>(context), IDeliveryDetailRepository
    {
        public async Task<IEnumerable<DeliveryDetail>> GetByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken)
        {
            return await DbContext.DeliveryDetails
                .Where(dd => dd.DeliveryId == deliveryId)
                .ToListAsync(cancellationToken);
        }
    }
}