// Godown.cs
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Godown : BaseDomainEntity
    {
        public required string GodownName { get; set; }
        public required string Location { get; set; }
        public string? GodownManager { get; set; }

        public ICollection<GodownInventory> GodownInventories { get; set; } = new List<GodownInventory>();
        public ICollection<InwardTransaction> InwardTransactions { get; set; } = new List<InwardTransaction>();
        public ICollection<OutwardTransaction> OutwardTransactions { get; set; } = new List<OutwardTransaction>();
        public ICollection<ReturnTransaction> ReturnTransactions { get; set; } = new List<ReturnTransaction>();
    }
}