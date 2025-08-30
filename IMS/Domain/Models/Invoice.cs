// Invoice.cs
using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Invoice : BaseDomainEntity
    {
        public required string InvoiceNumber { get; set; }
        public required int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public required int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public required DateTime InvoiceDate { get; set; }
        public required DateTime DueDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}