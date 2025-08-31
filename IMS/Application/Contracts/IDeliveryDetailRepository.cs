// Application/Contracts/IDeliveryDetailRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryDetailRepository : IGenericRepository<DeliveryDetail>
    {
        Task<DeliveryDetail?> GetDeliveryDetailWithDetailsAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<DeliveryDetail>> GetByDeliveryIdAsync(int deliveryId, CancellationToken cancellationToken);
        Task AddAsync(DeliveryDetail deliveryDetail, object cancellationationToken);
    }
}