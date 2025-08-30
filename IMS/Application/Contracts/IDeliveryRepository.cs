// Application/Contracts/IDeliveryRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        Task<Delivery?> GetDeliveryWithDetailsAsync(int id, CancellationToken cancellationToken);
    }
}