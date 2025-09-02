// Supplier.cs
using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Supplier : BaseDomainEntity
    {
        public required string SupplierName { get; set; }
        public required string ContactPerson { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
        public string? Address { get; set; }

        public ICollection<InwardTransaction> InwardTransactions { get; set; } = new List<InwardTransaction>();
    }
}