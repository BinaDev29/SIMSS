// InventoryAlert.cs - Smart alert system for inventory management
using Domain.Common;
using System;

namespace Domain.Models
{
    public class InventoryAlert : BaseDomainEntity
    {
        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }
        
        public required string AlertType { get; set; } // LOW_STOCK, OVERSTOCK, EXPIRY, REORDER
        public required string Severity { get; set; } // LOW, MEDIUM, HIGH, CRITICAL
        public required string Message { get; set; }
        public required DateTime AlertDate { get; set; }
        
        public decimal CurrentStock { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? RecommendedAction { get; set; }
        
        public bool IsActive { get; set; } = true;
        public bool IsAcknowledged { get; set; } = false;
        public DateTime? AcknowledgedDate { get; set; }
        public string? AcknowledgedBy { get; set; }
        
        public decimal ConfidenceScore { get; set; } = 1.0m;
        public string? ActionTaken { get; set; }
        public string? Notes { get; set; }
    }
}