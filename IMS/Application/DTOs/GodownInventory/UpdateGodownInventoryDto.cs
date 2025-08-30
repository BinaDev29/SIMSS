namespace Application.DTOs.GodownInventory
{
    public class UpdateGodownInventoryDto
    {
        public required int Id { get; set; }
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}