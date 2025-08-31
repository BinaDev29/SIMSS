using Application.DTOs.Common;

namespace Application.DTOs.Common
{
    public class CustomerQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? City { get; set; }
    }

    public class EmployeeQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public bool? IsActive { get; set; }
    }

    public class GodownQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public string? GodownType { get; set; }
    }

    public class InvoiceQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }

    public class InwardTransactionQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? ItemId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ItemQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public decimal? MinStock { get; set; }
        public decimal? MaxStock { get; set; }
        public bool? LowStockOnly { get; set; }
    }

    public class OutwardTransactionQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? ItemId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ReturnTransactionQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? ItemId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ReturnReason { get; set; }
    }

    public class SupplierQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? City { get; set; }
    }

    public class UserQueryParameters : PagedRequestDto
    {
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}