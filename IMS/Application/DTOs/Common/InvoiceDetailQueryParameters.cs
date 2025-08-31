using Application.DTOs.Common;

namespace Application.DTOs.Common
{
    public class InvoiceDetailQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? InvoiceId { get; set; }
        public int? ItemId { get; set; }
        public int? GodownId { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
    }
}