// Employee.cs
using Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Employee : BaseDomainEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? JoinDate { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? Salary { get; set; } // Salary ን ጨምረናል

        public required int UserId { get; set; }
        public virtual User? User { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<OutwardTransaction> OutwardTransactions { get; set; } = new List<OutwardTransaction>();
        public ICollection<ReturnTransaction> ReturnTransactions { get; set; } = new List<ReturnTransaction>();
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}