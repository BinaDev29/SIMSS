using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Invoice
{
    public class CreateInvoiceDetailDto
    {
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}