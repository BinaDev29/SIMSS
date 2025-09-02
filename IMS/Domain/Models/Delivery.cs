// Domain/Models/Delivery.cs
using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Delivery : BaseDomainEntity
    {
        public string TrackingNumber { get; set; } = string.Empty;
        public int OutwardTransactionId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;

        public virtual Customer? Customer { get; set; }
        public virtual OutwardTransaction? OutwardTransaction { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
    }
}