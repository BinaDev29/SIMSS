namespace Application.DTOs.Reports
{
    public class SalesReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalInvoices { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public int TotalInwardTransactions { get; set; }
        public int TotalOutwardTransactions { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}