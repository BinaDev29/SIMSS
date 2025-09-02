// InventoryAnalytics.cs - Analytics and reporting data
using Domain.Common;
using System;

namespace Domain.Models
{
    public class InventoryAnalytics : BaseDomainEntity
    {
        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }
        
        public required DateTime AnalysisDate { get; set; }
        public required string AnalysisPeriod { get; set; } // DAILY, WEEKLY, MONTHLY, YEARLY
        
        // ABC Analysis
        public string? ABCCategory { get; set; } // A, B, C
        public decimal ABCValue { get; set; }
        public decimal ABCPercentage { get; set; }
        
        // XYZ Analysis
        public string? XYZCategory { get; set; } // X, Y, Z
        public decimal DemandVariability { get; set; }
        public decimal CoefficientOfVariation { get; set; }
        
        // Performance Metrics
        public decimal TurnoverRate { get; set; }
        public decimal ServiceLevel { get; set; }
        public decimal FillRate { get; set; }
        public decimal StockoutFrequency { get; set; }
        public decimal CarryingCost { get; set; }
        public decimal OrderingCost { get; set; }
        
        // Velocity Analysis
        public decimal FastMoving { get; set; }
        public decimal SlowMoving { get; set; }
        public decimal NonMoving { get; set; }
        public int DaysInStock { get; set; }
        
        // Financial Metrics
        public decimal InventoryValue { get; set; }
        public decimal DeadStockValue { get; set; }
        public decimal ExcessStockValue { get; set; }
        public decimal OptimalStockValue { get; set; }

        // Add backward compatibility properties as settable
        public string AnalysisType { get; set; } = "General";
        public string Category { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Metrics { get; set; } = string.Empty;
    }
}