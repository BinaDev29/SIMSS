// InwardTransaction.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Domain.Models
{
    public class InwardTransaction : BaseDomainEntity
    {


        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }

        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public required int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public required int QuantityReceived { get; set; }
        public required DateTime InwardDate { get; set; }
        public string? Source { get; set; }
        public string? InvoiceNumber { get; set; }
        public int EmployeeId { get; set; }
    }
}