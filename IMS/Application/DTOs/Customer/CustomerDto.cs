using Application.DTOs.Delivery;
using Application.DTOs.Invoice;
using Application.DTOs.Transaction;
using System.Collections.Generic;

namespace Application.DTOs.Customer
{
    public record CustomerDto
    {
        public int Id { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public string ContactPerson { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? Address { get; init; }
        public string? TaxId { get; init; }
        public string? PaymentTerms { get; init; }

        // እነዚህ ከdomain modelህ ጋር የሚመሳሰሉ collections ናቸው
        public ICollection<InvoiceDto> Invoices { get; init; } = new List<InvoiceDto>();
        public ICollection<DeliveryDto> Deliveries { get; init; } = new List<DeliveryDto>();
        public ICollection<OutwardTransactionDto> OutwardTransactions { get; init; } = new List<OutwardTransactionDto>();
        public ICollection<ReturnTransactionDto> ReturnTransactions { get; init; } = new List<ReturnTransactionDto>();
    }
}