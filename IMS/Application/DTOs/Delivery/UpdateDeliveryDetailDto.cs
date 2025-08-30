namespace Application.DTOs.Delivery
{
    public class UpdateDeliveryDetailDto
    {
        public required int Id { get; set; }
        public required int DeliveryId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}