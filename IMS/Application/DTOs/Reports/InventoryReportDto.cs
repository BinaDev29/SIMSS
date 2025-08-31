namespace Application.DTOs.Reports
{
    public class InventoryReportDto
    {
        public int TotalItems { get; set; }
        public decimal TotalValue { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<ItemReportDto> Items { get; set; } = [];
    }

    public class ItemReportDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsLowStock { get; set; }
    }
}