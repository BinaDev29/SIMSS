using Application.Contracts;
using Application.DTOs.Reports;
using Application.Services;
using Domain.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportingService(
        IGenericRepository<Item> itemRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<Invoice> invoiceRepository,
        IGenericRepository<InwardTransaction> inwardTransactionRepository,
        IGenericRepository<OutwardTransaction> outwardTransactionRepository,
        IGenericRepository<GodownInventory> godownInventoryRepository,
        IMapper mapper) : IReportingService
    {
        public async Task<InventoryReportDto> GenerateStockReportAsync(StockReportParametersDto parameters)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            
            if (parameters.ItemId.HasValue)
                items = items.Where(x => x.Id == parameters.ItemId.Value).ToList();
            
            if (!string.IsNullOrEmpty(parameters.Category))
                items = items.Where(x => x.Category == parameters.Category).ToList();

            return new InventoryReportDto
            {
                TotalItems = items.Count,
                TotalValue = items.Sum(x => x.Price * x.Quantity),
                LowStockItems = items.Count(x => x.Quantity < x.MinimumStockLevel),
                OutOfStockItems = items.Count(x => x.Quantity == 0),
                GeneratedAt = DateTime.UtcNow,
                Items = items.Select(x => new ItemReportDto
                {
                    ItemName = x.ItemName,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    TotalValue = x.Price * x.Quantity,
                    IsLowStock = x.Quantity < x.MinimumStockLevel
                }).ToList()
            };
        }

        public async Task<SalesReportDto> GenerateSalesReportAsync(SalesReportParametersDto parameters)
        {
            var invoices = await invoiceRepository.GetAllAsync(CancellationToken.None);
            var filteredInvoices = invoices.Where(x => x.DateCreated >= parameters.StartDate && x.DateCreated <= parameters.EndDate);

            if (parameters.CustomerId.HasValue)
                filteredInvoices = filteredInvoices.Where(x => x.CustomerId == parameters.CustomerId.Value);

            return new SalesReportDto
            {
                StartDate = parameters.StartDate,
                EndDate = parameters.EndDate,
                TotalInvoices = filteredInvoices.Count(),
                TotalSalesAmount = filteredInvoices.Sum(x => x.TotalAmount),
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<StockMovementReportDto> GenerateStockMovementReportAsync(StockMovementParametersDto parameters)
        {
            var inwardTransactions = await inwardTransactionRepository.GetAllAsync(CancellationToken.None);
            var outwardTransactions = await outwardTransactionRepository.GetAllAsync(CancellationToken.None);

            var filteredInward = inwardTransactions.Where(x => x.DateCreated >= parameters.StartDate && x.DateCreated <= parameters.EndDate);
            var filteredOutward = outwardTransactions.Where(x => x.DateCreated >= parameters.StartDate && x.DateCreated <= parameters.EndDate);

            if (parameters.ItemId.HasValue)
            {
                filteredInward = filteredInward.Where(x => x.ItemId == parameters.ItemId.Value);
                filteredOutward = filteredOutward.Where(x => x.ItemId == parameters.ItemId.Value);
            }

            var movements = new List<MovementDetailDto>();
            
            movements.AddRange(filteredInward.Select(x => new MovementDetailDto
            {
                Date = x.DateCreated,
                ItemName = x.Item?.ItemName ?? "Unknown",
                MovementType = "Inward",
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalValue = x.Quantity * x.UnitPrice,
                Reference = $"IN-{x.Id}"
            }));

            movements.AddRange(filteredOutward.Select(x => new MovementDetailDto
            {
                Date = x.DateCreated,
                ItemName = x.Item?.ItemName ?? "Unknown",
                MovementType = "Outward",
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalValue = x.Quantity * x.UnitPrice,
                Reference = $"OUT-{x.Id}"
            }));

            return new StockMovementReportDto
            {
                StartDate = parameters.StartDate,
                EndDate = parameters.EndDate,
                TotalMovements = movements.Count,
                InwardMovements = movements.Count(x => x.MovementType == "Inward"),
                OutwardMovements = movements.Count(x => x.MovementType == "Outward"),
                TotalInwardValue = movements.Where(x => x.MovementType == "Inward").Sum(x => x.TotalValue),
                TotalOutwardValue = movements.Where(x => x.MovementType == "Outward").Sum(x => x.TotalValue),
                GeneratedAt = DateTime.UtcNow,
                Movements = movements.OrderBy(x => x.Date).ToList()
            };
        }

        public async Task<ValuationReportDto> GenerateValuationReportAsync(ValuationReportParametersDto parameters)
        {
            var inventories = await godownInventoryRepository.GetAllAsync(CancellationToken.None);
            
            if (parameters.GodownId.HasValue)
                inventories = inventories.Where(x => x.GodownId == parameters.GodownId.Value).ToList();

            return new ValuationReportDto
            {
                AsOfDate = parameters.AsOfDate,
                ValuationMethod = parameters.ValuationMethod ?? "FIFO",
                TotalItems = inventories.Count,
                TotalValue = inventories.Sum(x => x.Quantity * (x.Item?.Price ?? 0)),
                GeneratedAt = DateTime.UtcNow,
                Items = inventories.Select(x => new ItemValuationDto
                {
                    ItemName = x.Item?.ItemName ?? "Unknown",
                    GodownName = x.Godown?.GodownName ?? "Unknown",
                    Quantity = x.Quantity,
                    UnitValue = x.Item?.Price ?? 0,
                    TotalValue = x.Quantity * (x.Item?.Price ?? 0),
                    Category = x.Item?.Category ?? "Unknown"
                }).ToList()
            };
        }

        public async Task<IEnumerable<ReportSummaryDto>> GetReportHistoryAsync(int userId)
        {
            // This would typically come from a reports table in the database
            // For now, return empty list as we don't have a reports history table
            await Task.CompletedTask;
            return [];
        }

        public async Task<byte[]> ExportReportToPdfAsync(int reportId)
        {
            // PDF export implementation would go here
            // For now, return empty byte array
            await Task.CompletedTask;
            return [];
        }

        public async Task<byte[]> ExportReportToExcelAsync(int reportId)
        {
            // Excel export implementation would go here
            // For now, return empty byte array
            await Task.CompletedTask;
            return [];
        }
    }
}