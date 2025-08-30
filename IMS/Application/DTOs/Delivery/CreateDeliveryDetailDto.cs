namespace Application.DTOs.Delivery
{
    public class CreateDeliveryDetailDto
    {
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}