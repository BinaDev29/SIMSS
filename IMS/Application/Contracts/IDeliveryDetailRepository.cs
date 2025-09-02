// Application/Contracts/IDeliveryDetailRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryDetailRepository : IGenericRepository<DeliveryDetail>
    {
        Task<IReadOnlyList<DeliveryDetail>> GetDetailsByDeliveryAsync(int deliveryId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DeliveryDetail>> GetDetailsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<PagedResult<DeliveryDetail>> GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}