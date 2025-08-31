using System.Threading.Tasks;
using System.Threading;

namespace Application.Services
{
    public interface IGodownInventoryService
    {
        Task UpdateInventoryQuantity(int godownId, int itemId, int quantityChange, CancellationToken cancellationToken);
        Task<bool> CheckSufficientStock(int itemId, int godownId, int requiredQuantity, CancellationToken cancellationToken);
        Task<bool> CheckSufficientStock(object value1, object value2, object value3, CancellationToken cancellationToken);
    }
}