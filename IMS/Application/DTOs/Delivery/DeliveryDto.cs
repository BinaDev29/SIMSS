using Application.DTOs.Common;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Delivery
{
    public class DeliveryDto : BaseDto
    {
        public required string DeliveryNumber { get; set; }
        public required DateTime DeliveryDate { get; set; }
        public required int CustomerId { get; set; }
        public required int DeliveredByEmployeeId { get; set; }
        public string? Status { get; set; }
        public ICollection<DeliveryDetailDto> DeliveryDetails { get; set; } = new List<DeliveryDetailDto>();
    }
}