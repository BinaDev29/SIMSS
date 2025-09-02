// Application/Contracts/IDeliveryRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        Task<Delivery?> GetDeliveryWithDetailsAsync(int id, CancellationToken cancellationToken);
        Task<Delivery?> GetByOutwardTransactionIdAsync(int outwardTransactionId, CancellationToken cancellationToken);
        Task<PagedResult<Delivery>> GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}
