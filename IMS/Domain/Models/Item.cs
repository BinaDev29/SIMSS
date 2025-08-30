// Item.cs
using Domain.Common;
using System.Collections.Generic;
using System;

namespace Domain.Models
{
    public class Item : BaseDomainEntity
    {
        public required string ItemName { get; set; }
        public required string ItemCode { get; set; }
        public required decimal StockQuantity { get; set; }
        public required string UnitOfMeasure { get; set; }
        public string? Description { get; set; }
        public required decimal PurchasePrice { get; set; }
        public required decimal SalePrice { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public ICollection<GodownInventory> GodownInventories { get; set; } = new List<GodownInventory>();
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
        public ICollection<InwardTransaction> InwardTransactions { get; set; } = new List<InwardTransaction>();
        public ICollection<OutwardTransaction> OutwardTransactions { get; set; } = new List<OutwardTransaction>();
        public ICollection<ReturnTransaction> ReturnTransactions { get; set; } = new List<ReturnTransaction>();
    }
}