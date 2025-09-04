// Domain/Item.cs
using Domain.Common;
using System.Collections.Generic;
using System;

namespace Domain.Models
{
    public class Item : BaseDomainEntity
    {
        public required string ItemName { get; set; }
        public required string ItemCode { get; set; }
        public decimal StockQuantity { get; set; }
        public required string UnitOfMeasure { get; set; }
        public string? Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal MinimumStockLevel { get; set; }
        public decimal MaximumStockLevel { get; set; }
        public decimal ReorderLevel { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DateModified { get; set; }

        // Foreign Key
        public int SupplierId { get; set; }

        // Navigation Properties
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<GodownInventory> GodownInventories { get; set; } = new List<GodownInventory>();
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
        public virtual ICollection<InwardTransaction> InwardTransactions { get; set; } = new List<InwardTransaction>();
        public virtual ICollection<OutwardTransaction> OutwardTransactions { get; set; } = new List<OutwardTransaction>();
        public virtual ICollection<ReturnTransaction> ReturnTransactions { get; set; } = new List<ReturnTransaction>();
    }
}