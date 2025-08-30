using Application.DTOs.Common;
using System;

namespace Application.DTOs.Transaction
{
    public class OutwardTransactionDto : BaseDto
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int CustomerId { get; set; }
        public required int QuantityDelivered { get; set; }
        public required DateTime OutwardDate { get; set; }
        public required string Destination { get; set; }
        public required string InvoiceNumber { get; set; }
        public required int EmployeeId { get; set; }
    }
}