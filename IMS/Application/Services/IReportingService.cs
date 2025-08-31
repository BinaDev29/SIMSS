using Application.DTOs.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IReportingService
    {
        Task<InventoryReportDto> GenerateStockReportAsync(StockReportParametersDto parameters);
        Task<SalesReportDto> GenerateSalesReportAsync(SalesReportParametersDto parameters);
        Task<StockMovementReportDto> GenerateStockMovementReportAsync(StockMovementParametersDto parameters);
        Task<ValuationReportDto> GenerateValuationReportAsync(ValuationReportParametersDto parameters);
        Task<IEnumerable<ReportSummaryDto>> GetReportHistoryAsync(int userId);
        Task<byte[]> ExportReportToPdfAsync(int reportId);
        Task<byte[]> ExportReportToExcelAsync(int reportId);
    }
}