// Persistence/Repositories/InventoryAlertRepository.cs
using Domain.Models;

namespace Persistence.Repositories
{
    public class PagedResult<T>
    {
        private List<InventoryAlert> items;
        private int totalCount;
        private int pageNumber;
        private int pageSize;

        public PagedResult(List<InventoryAlert> items, int totalCount, int pageNumber, int pageSize)
        {
            this.items = items;
            this.totalCount = totalCount;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }
    }
}