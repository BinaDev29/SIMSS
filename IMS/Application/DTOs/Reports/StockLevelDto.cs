namespace Application.DTOs.Reports
{
    public class StockLevelDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumLevel { get; set; }
        public int MaximumLevel { get; set; }
        public string StockStatus { get; set; } = string.Empty;
    }
}