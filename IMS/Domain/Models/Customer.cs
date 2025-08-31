// Customer.cs
using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Customer : BaseDomainEntity
    {
        public required string CustomerName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ContactPerson { get; set; }
        public required string PhoneNumber { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public string? Address { get; set; }
        public string? TaxId { get; set; }
        public string? PaymentTerms { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
        public ICollection<OutwardTransaction> OutwardTransactions { get; set; } = [];
        public ICollection<ReturnTransaction> ReturnTransactions { get; set; } = [];
    }
}