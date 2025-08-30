// Application/Contracts/IDeliveryDetailRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryDetailRepository : IGenericRepository<DeliveryDetail>
    {
        Task<IEnumerable<DeliveryDetail>> GetByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken);
    }
}