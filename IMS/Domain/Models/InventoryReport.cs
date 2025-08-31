using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class InventoryReport : BaseDomainEntity
    {
        public required string ReportName { get; set; }
        public required string ReportType { get; set; } // "Stock", "Sales", "Movement", "Valuation"
        public required DateTime GeneratedDate { get; set; }
        public required DateTime FromDate { get; set; }
        public required DateTime ToDate { get; set; }
        public string? Parameters { get; set; } // JSON string for report parameters
        public string? ReportData { get; set; } // JSON string for report data
        public required string GeneratedBy { get; set; }
        public string? FilePath { get; set; }
        public string? Status { get; set; } // "Generating", "Completed", "Failed"
    }
}