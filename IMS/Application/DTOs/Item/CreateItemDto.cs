using System;

namespace Application.DTOs.Item
{
    public class CreateItemDto
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
    }
}