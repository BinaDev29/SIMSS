using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Invoice
{
    public class UpdateInvoiceDetailDto
    {
        public required int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}