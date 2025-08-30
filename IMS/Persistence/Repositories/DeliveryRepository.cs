// Persistence/Repositories/DeliveryRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DeliveryRepository(SIMSDbContext dbContext) : GenericRepository<Delivery>(dbContext), IDeliveryRepository
    {
        public async Task<Delivery?> GetDeliveryWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await DbContext.Deliveries
                .Include(d => d.DeliveryDetails)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }
    }
}