using System;
using System.Collections.Generic;

namespace Application.DTOs.Invoice
{
    public class UpdateInvoiceDto
    {
        public required int Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public ICollection<UpdateInvoiceDetailDto>? InvoiceDetails { get; set; } = new List<UpdateInvoiceDetailDto>();
    }
}