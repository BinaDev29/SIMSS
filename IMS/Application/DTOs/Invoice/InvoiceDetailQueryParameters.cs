using Application.DTOs.Common;

namespace Application.DTOs.Invoice
{
    public class InvoiceDetailQueryParameters : QueryParameters
    {
        public string? SearchTerm { get; set; }
        public int? InvoiceId { get; set; }
        public int? ItemId { get; set; }
        public int? GodownId { get; set; }
    }
}