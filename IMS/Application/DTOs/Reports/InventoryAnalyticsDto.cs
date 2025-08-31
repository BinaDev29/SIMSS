namespace Application.DTOs.Reports
{
    public class InventoryAnalyticsDto
    {
        public int TotalItems { get; set; }
        public decimal TotalStockValue { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int TotalInwardTransactions { get; set; }
        public int TotalOutwardTransactions { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}