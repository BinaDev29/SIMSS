using Application.DTOs.Common;

namespace Application.DTOs.Delivery
{
    public class DeliveryDetailDto : BaseDto
    {
        public required int DeliveryId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
    }
}