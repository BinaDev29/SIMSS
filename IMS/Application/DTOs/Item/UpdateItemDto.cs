using System;

namespace Application.DTOs.Item
{
    public class UpdateItemDto
    {
        public required int Id { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public decimal? StockQuantity { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Description { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}