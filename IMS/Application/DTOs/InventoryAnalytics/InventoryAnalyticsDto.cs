// Application/DTOs/InventoryAnalytics/InventoryAnalyticsDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.InventoryAnalytics
{
    public class InventoryAnalyticsDto : BaseDto
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int GodownId { get; set; }
        public string? GodownName { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string AnalysisPeriod { get; set; } = string.Empty;
        public string? ABCCategory { get; set; }
        public decimal ABCValue { get; set; }
        public decimal ABCPercentage { get; set; }
        public string? XYZCategory { get; set; }
        public decimal DemandVariability { get; set; }
        public decimal CoefficientOfVariation { get; set; }
        public decimal TurnoverRate { get; set; }
        public decimal ServiceLevel { get; set; }
        public decimal FillRate { get; set; }
        public decimal StockoutFrequency { get; set; }
        public decimal CarryingCost { get; set; }
        public decimal OrderingCost { get; set; }
        public decimal FastMoving { get; set; }
        public decimal SlowMoving { get; set; }
        public decimal NonMoving { get; set; }
        public int DaysInStock { get; set; }
        public decimal InventoryValue { get; set; }
        public decimal DeadStockValue { get; set; }
        public decimal ExcessStockValue { get; set; }
        public decimal OptimalStockValue { get; set; }
    }
}