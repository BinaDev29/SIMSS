using Application.DTOs.Common;
using System;

namespace Application.DTOs.Transaction
{
    public class InwardTransactionDto : BaseDto
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int SupplierId { get; set; }
        public required int QuantityReceived { get; set; }
        public required DateTime InwardDate { get; set; }
        public string? Source { get; set; }
        public string? InvoiceNumber { get; set; }
    }
}