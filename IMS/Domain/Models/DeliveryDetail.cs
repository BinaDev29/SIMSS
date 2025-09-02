// DeliveryDetail.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class DeliveryDetail : BaseDomainEntity
    {
        public required int DeliveryId { get; set; }
        public virtual Delivery? Delivery { get; set; }

        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public required int Quantity { get; set; }
        
        // Add GodownId property that was missing
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }
    }
}