using Application.DTOs.Reports;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportsController(IReportingService reportingService)
        {
            ArgumentNullException.ThrowIfNull(reportingService);
            _reportingService = reportingService;
        }

        [HttpPost("stock")]
        [ProducesResponseType(typeof(InventoryReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InventoryReportDto>> GenerateStockReport([FromBody] StockReportParametersDto parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Report parameters are required.");
            }

            if (parameters.FromDate > parameters.ToDate)
            {
                return BadRequest("From date cannot be greater than to date.");
            }

            var report = await _reportingService.GenerateStockReportAsync(parameters);
            return Ok(report);
        }

        [HttpPost("sales")]
        [ProducesResponseType(typeof(SalesReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SalesReportDto>> GenerateSalesReport([FromBody] SalesReportParametersDto parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Report parameters are required.");
            }

            if (parameters.FromDate > parameters.ToDate)
            {
                return BadRequest("From date cannot be greater than to date.");
            }

            var report = await _reportingService.GenerateSalesReportAsync(parameters);
            return Ok(report);
        }

        [HttpPost("stock-movement")]
        [ProducesResponseType(typeof(StockMovementReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<StockMovementReportDto>> GenerateStockMovementReport([FromBody] StockMovementParametersDto parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Report parameters are required.");
            }

            if (parameters.FromDate > parameters.ToDate)
            {
                return BadRequest("From date cannot be greater than to date.");
            }

            var report = await _reportingService.GenerateStockMovementReportAsync(parameters);
            return Ok(report);
        }

        [HttpPost("valuation")]
        [ProducesResponseType(typeof(ValuationReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ValuationReportDto>> GenerateValuationReport([FromBody] ValuationReportParametersDto parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Report parameters are required.");
            }

            var report = await _reportingService.GenerateValuationReportAsync(parameters);
            return Ok(report);
        }

        [HttpGet("history/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ReportSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ReportSummaryDto>>> GetReportHistory(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var reports = await _reportingService.GetReportHistoryAsync(userId);
            return Ok(reports);
        }

        [HttpGet("{reportId:int}/export/pdf")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ExportToPdf(int reportId)
        {
            try
            {
                var pdfBytes = await _reportingService.ExportReportToPdfAsync(reportId);
                return File(pdfBytes, "application/pdf", $"report_{reportId}.pdf");
            }
            catch (NotImplementedException)
            {
                return BadRequest("PDF export functionality is not yet implemented.");
            }
            catch (Exception)
            {
                return NotFound($"Report with ID {reportId} not found.");
            }
        }

        [HttpGet("{reportId:int}/export/excel")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ExportToExcel(int reportId)
        {
            try
            {
                var excelBytes = await _reportingService.ExportReportToExcelAsync(reportId);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"report_{reportId}.xlsx");
            }
            catch (NotImplementedException)
            {
                return BadRequest("Excel export functionality is not yet implemented.");
            }
            catch (Exception)
            {
                return NotFound($"Report with ID {reportId} not found.");
            }
        }
    }
}