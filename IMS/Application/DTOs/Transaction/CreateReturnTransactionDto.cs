using System;

namespace Application.DTOs.Transaction
{
    public class CreateReturnTransactionDto
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int CustomerId { get; set; }
        public required int Quantity { get; set; }
        public required DateTime ReturnDate { get; set; }
        public required string Reason { get; set; }
        public required int EmployeeId { get; set; }
    }
}