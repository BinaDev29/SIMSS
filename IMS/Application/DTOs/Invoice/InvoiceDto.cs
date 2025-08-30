using Application.DTOs.Common;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Invoice
{
    public class InvoiceDto : BaseDto
    {
        public required string InvoiceNumber { get; set; }
        public required int CustomerId { get; set; }
        public required int EmployeeId { get; set; }
        public required DateTime InvoiceDate { get; set; }
        public required DateTime DueDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public ICollection<InvoiceDetailDto> InvoiceDetails { get; set; } = new List<InvoiceDetailDto>();
    }
}