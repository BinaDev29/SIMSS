// InvoiceDetail.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class InvoiceDetail : BaseDomainEntity
    {
        public required int InvoiceId { get; set; }
        public virtual Invoice? Invoice { get; set; }

        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}