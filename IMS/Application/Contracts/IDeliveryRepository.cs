// Application/Contracts/IDeliveryRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        Task<Delivery?> GetDeliveryByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken);
        Task<IReadOnlyList<Delivery>> GetDeliveriesByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Delivery>> GetDeliveriesByStatusAsync(string status, CancellationToken cancellationToken);
        Task<PagedResult<Delivery>> GetPagedDeliveriesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
    }
}