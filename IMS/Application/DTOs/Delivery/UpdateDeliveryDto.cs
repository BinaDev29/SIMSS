using System;
using System.Collections.Generic;

namespace Application.DTOs.Delivery
{
    public class UpdateDeliveryDto
    {
        public required int Id { get; set; }
        public required string DeliveryNumber { get; set; }
        public required DateTime DeliveryDate { get; set; }
        public required int CustomerId { get; set; }
        public required int DeliveredByEmployeeId { get; set; }
        public string? Status { get; set; }
        public ICollection<UpdateDeliveryDetailDto> DeliveryDetails { get; set; } = new List<UpdateDeliveryDetailDto>();
    }
}