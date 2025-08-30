// OutwardTransaction.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Domain.Models
{
    public class OutwardTransaction : BaseDomainEntity
    {
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }

        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public required int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public required int QuantityDelivered { get; set; }
        public required DateTime OutwardDate { get; set; }
        public required string Destination { get; set; }
        public required string InvoiceNumber { get; set; }
        public required int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}