// Application/Contracts/IDeliveryDetailRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryDetailRepository : IGenericRepository<DeliveryDetail>
    {
        Task<DeliveryDetail> AddAsync(DeliveryDetail deliveryDetail, CancellationToken cancellationToken = default);
        // Add the missing method signature
        Task<DeliveryDetail?> GetDeliveryDetailWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}