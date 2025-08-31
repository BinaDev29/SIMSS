using Domain.Common;
using System;

namespace Domain.Models
{
    public class BatchOperation : BaseDomainEntity
    {
        public required string OperationType { get; set; } // "Import", "Export", "Update", "Delete"
        public required string EntityType { get; set; } // "Item", "Customer", "Supplier", etc.
        public required string Status { get; set; } // "Pending", "Processing", "Completed", "Failed"
        public required int TotalRecords { get; set; }
        public int ProcessedRecords { get; set; } = 0;
        public int SuccessfulRecords { get; set; } = 0;
        public int FailedRecords { get; set; } = 0;
        public string? ErrorLog { get; set; } // JSON string for error details
        public string? FilePath { get; set; }
        public required string InitiatedBy { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Parameters { get; set; } // JSON string for operation parameters
    }
}