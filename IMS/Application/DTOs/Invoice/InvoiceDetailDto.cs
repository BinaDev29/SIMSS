using Application.DTOs.Common;

namespace Application.DTOs.Invoice
{
    public class InvoiceDetailDto : BaseDto
    {
        public required int InvoiceId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}