// Application/Services/GodownInventoryService.cs

using Application.Contracts;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GodownInventoryService(IGodownInventoryRepository godownInventoryRepository) : IGodownInventoryService
    {
        public async Task<bool> CheckSufficientStock(int itemId, int godownId, int requiredQuantity, CancellationToken cancellationToken)
        {
            var inventory = await godownInventoryRepository.GetByGodownAndItemAsync(godownId, itemId, cancellationToken);
            return inventory != null && inventory.Quantity >= requiredQuantity;
        }

        public async Task UpdateInventoryQuantity(int godownId, int itemId, int quantityChange, CancellationToken cancellationToken)
        {
            var inventory = await godownInventoryRepository.GetByGodownAndItemAsync(godownId, itemId, cancellationToken);
            if (inventory != null)
            {
                inventory.Quantity += quantityChange;
                await godownInventoryRepository.UpdateAsync(inventory, cancellationToken);
            }
        }
    }
}