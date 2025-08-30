// Delivery.cs
using Domain.Common;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Index(nameof(DeliveryNumber), IsUnique = true)]
    public class Delivery : BaseDomainEntity
    {
        public required string DeliveryNumber { get; set; }
        public required DateTime DeliveryDate { get; set; }

        public required int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public required int DeliveredByEmployeeId { get; set; }
        public virtual Employee? DeliveredBy { get; set; }

        public string? Status { get; set; }

        public ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
    }
}