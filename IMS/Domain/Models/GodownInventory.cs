// GodownInventory.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class GodownInventory : BaseDomainEntity
    {
        public required int GodownId { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }

        public virtual Godown? Godown { get; set; }
        public virtual Item? Item { get; set; }
    }
}