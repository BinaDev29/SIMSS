namespace Application.DTOs.GodownInventory
{
    public class CreateGodownInventoryDto
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}