using Application.DTOs.Common;

namespace Application.DTOs.GodownInventory
{
    public class GodownInventoryDto : BaseDto
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}