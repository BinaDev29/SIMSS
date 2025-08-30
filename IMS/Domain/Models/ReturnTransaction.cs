// ReturnTransaction.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Domain.Models
{
    public class ReturnTransaction : BaseDomainEntity
    {
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }

        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public required int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public required int Quantity { get; set; }
        public required DateTime ReturnDate { get; set; }
        public required string Reason { get; set; }
        public required int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}