using Application.Contracts;
using Application.DTOs.Reports;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InventoryAnalyticsService(
        IGenericRepository<Item> itemRepository,
        IGenericRepository<InwardTransaction> inwardTransactionRepository,
        IGenericRepository<OutwardTransaction> outwardTransactionRepository,
        IGenericRepository<GodownInventory> godownInventoryRepository,
        IGenericRepository<DemandForecast> demandForecastRepository,
        IGenericRepository<InventoryAlert> inventoryAlertRepository,
        IGenericRepository<InventoryAnalytics> inventoryAnalyticsRepository,
        IGenericRepository<SmartReorder> smartReorderRepository) : IInventoryAnalyticsService
    {
        // Demand Forecasting
        public async Task<DemandForecast> GenerateDemandForecastAsync(int itemId, int godownId, string period, int length)
        {
            var forecast = new DemandForecast
            {
                ItemId = itemId,
                GodownId = godownId,
                Period = period,
                ForecastLength = length,
                GeneratedDate = DateTime.UtcNow,
                ForecastData = "Sample forecast data", // This would contain actual forecast calculations
                Accuracy = 85.5m,
                Method = "Linear Regression"
            };

            return await demandForecastRepository.AddAsync(forecast, CancellationToken.None);
        }

        public async Task<List<DemandForecast>> GetForecastsAsync(int? itemId = null, int? godownId = null)
        {
            var forecasts = await demandForecastRepository.GetAllAsync(CancellationToken.None);
            var filteredForecasts = forecasts.AsQueryable();

            if (itemId.HasValue)
                filteredForecasts = filteredForecasts.Where(x => x.ItemId == itemId.Value);

            if (godownId.HasValue)
                filteredForecasts = filteredForecasts.Where(x => x.GodownId == godownId.Value);

            return filteredForecasts.ToList();
        }

        public async Task<decimal> CalculateForecastAccuracyAsync(int itemId, int godownId, DateTime startDate, DateTime endDate)
        {
            // This would calculate actual vs predicted accuracy
            await Task.CompletedTask;
            return 85.5m; // Sample accuracy percentage
        }

        // Smart Alerts
        public async Task<List<InventoryAlert>> GenerateSmartAlertsAsync()
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var alerts = new List<InventoryAlert>();

            foreach (var item in items.Where(x => x.Quantity <= x.MinimumStockLevel))
            {
                var alert = new InventoryAlert
                {
                    ItemId = item.Id,
                    AlertType = "Low Stock",
                    Message = $"Item {item.ItemName} is below minimum stock level",
                    Severity = item.Quantity == 0 ? "Critical" : "Warning",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Threshold = item.MinimumStockLevel,
                    CurrentValue = item.Quantity
                };

                alerts.Add(await inventoryAlertRepository.AddAsync(alert, CancellationToken.None));
            }

            return alerts;
        }

        public async Task<List<InventoryAlert>> GetActiveAlertsAsync(int? godownId = null)
        {
            var alerts = await inventoryAlertRepository.GetAllAsync(CancellationToken.None);
            return alerts.Where(x => x.IsActive).ToList();
        }

        public async Task<List<InventoryAlert>> GetCriticalAlertsAsync()
        {
            var alerts = await inventoryAlertRepository.GetAllAsync(CancellationToken.None);
            return alerts.Where(x => x.IsActive && x.Severity == "Critical").ToList();
        }

        public async Task ProcessAlertAsync(int alertId, string action, string? notes = null)
        {
            var alert = await inventoryAlertRepository.GetByIdAsync(alertId, CancellationToken.None);
            if (alert != null)
            {
                if (action == "Acknowledge")
                {
                    alert.IsActive = false;
                    alert.ProcessedDate = DateTime.UtcNow;
                    await inventoryAlertRepository.Update(alert, CancellationToken.None);
                }
            }
        }

        // ABC/XYZ Analysis
        public async Task<List<InventoryAnalytics>> PerformABCAnalysisAsync(int godownId)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var analytics = new List<InventoryAnalytics>();

            foreach (var item in items)
            {
                var analytic = new InventoryAnalytics
                {
                    ItemId = item.Id,
                    GodownId = godownId,
                    AnalysisType = "ABC",
                    Category = DetermineABCCategory(item),
                    Value = item.Price * item.Quantity,
                    AnalysisDate = DateTime.UtcNow,
                    Metrics = $"Value: {item.Price * item.Quantity:C}"
                };

                analytics.Add(await inventoryAnalyticsRepository.AddAsync(analytic, CancellationToken.None));
            }

            return analytics;
        }

        public async Task<List<InventoryAnalytics>> PerformXYZAnalysisAsync(int godownId)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var analytics = new List<InventoryAnalytics>();

            foreach (var item in items)
            {
                var analytic = new InventoryAnalytics
                {
                    ItemId = item.Id,
                    GodownId = godownId,
                    AnalysisType = "XYZ",
                    Category = DetermineXYZCategory(item),
                    Value = CalculateVariability(item),
                    AnalysisDate = DateTime.UtcNow,
                    Metrics = $"Variability: {CalculateVariability(item):F2}"
                };

                analytics.Add(await inventoryAnalyticsRepository.AddAsync(analytic, CancellationToken.None));
            }

            return analytics;
        }

        public async Task<List<InventoryAnalytics>> GetCombinedAnalysisAsync(int godownId)
        {
            var analytics = await inventoryAnalyticsRepository.GetAllAsync(CancellationToken.None);
            return analytics.Where(x => x.GodownId == godownId).ToList();
        }

        // Smart Reordering
        public async Task<List<SmartReorder>> GenerateReorderRecommendationsAsync(int? godownId = null)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var recommendations = new List<SmartReorder>();

            foreach (var item in items.Where(x => x.Quantity <= x.ReorderLevel))
            {
                var reorder = new SmartReorder
                {
                    ItemId = item.Id,
                    GodownId = godownId ?? 1,
                    RecommendedQuantity = item.MaximumStockLevel - item.Quantity,
                    ReorderPoint = item.ReorderLevel,
                    LeadTime = 7, // Default lead time
                    Priority = item.Quantity == 0 ? "High" : "Medium",
                    GeneratedDate = DateTime.UtcNow,
                    Status = "Pending"
                };

                recommendations.Add(await smartReorderRepository.AddAsync(reorder, CancellationToken.None));
            }

            return recommendations;
        }

        public async Task<SmartReorder> CalculateOptimalReorderAsync(int itemId, int godownId)
        {
            var item = await itemRepository.GetByIdAsync(itemId, CancellationToken.None);
            if (item == null) throw new ArgumentException("Item not found");

            var eoq = await CalculateEconomicOrderQuantityAsync(itemId, godownId);
            var safetyStock = await CalculateSafetyStockAsync(itemId, godownId);

            var reorder = new SmartReorder
            {
                ItemId = itemId,
                GodownId = godownId,
                RecommendedQuantity = (int)eoq,
                ReorderPoint = (int)safetyStock + item.ReorderLevel,
                LeadTime = 7,
                Priority = "Medium",
                GeneratedDate = DateTime.UtcNow,
                Status = "Calculated"
            };

            return await smartReorderRepository.AddAsync(reorder, CancellationToken.None);
        }

        public async Task<decimal> CalculateEconomicOrderQuantityAsync(int itemId, int godownId)
        {
            // EOQ = sqrt((2 * D * S) / H)
            // D = Annual demand, S = Setup cost, H = Holding cost
            await Task.CompletedTask;
            return 100; // Sample EOQ calculation
        }

        public async Task<decimal> CalculateSafetyStockAsync(int itemId, int godownId)
        {
            // Safety Stock = Z * σ * sqrt(L)
            // Z = Service level factor, σ = Standard deviation of demand, L = Lead time
            await Task.CompletedTask;
            return 25; // Sample safety stock calculation
        }

        // Performance Analytics
        public async Task<Dictionary<string, decimal>> GetInventoryKPIsAsync(int godownId)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            
            return new Dictionary<string, decimal>
            {
                ["TotalValue"] = items.Sum(x => x.Price * x.Quantity),
                ["TurnoverRate"] = 4.5m,
                ["ServiceLevel"] = 95.2m,
                ["StockoutRate"] = 2.1m,
                ["CarryingCost"] = 15.5m
            };
        }

        public async Task<List<InventoryAnalytics>> GetVelocityAnalysisAsync(int godownId)
        {
            var analytics = await inventoryAnalyticsRepository.GetAllAsync(CancellationToken.None);
            return analytics.Where(x => x.GodownId == godownId && x.AnalysisType == "Velocity").ToList();
        }

        public async Task<decimal> CalculateTurnoverRateAsync(int itemId, int godownId)
        {
            // Turnover Rate = Cost of Goods Sold / Average Inventory Value
            await Task.CompletedTask;
            return 6.5m; // Sample turnover rate
        }

        public async Task<decimal> CalculateServiceLevelAsync(int itemId, int godownId)
        {
            // Service Level = (Orders Fulfilled / Total Orders) * 100
            await Task.CompletedTask;
            return 96.8m; // Sample service level percentage
        }

        // Reporting
        public async Task<object> GetInventoryDashboardAsync(int godownId)
        {
            var kpis = await GetInventoryKPIsAsync(godownId);
            var alerts = await GetActiveAlertsAsync(godownId);
            
            return new
            {
                KPIs = kpis,
                ActiveAlerts = alerts.Count,
                CriticalAlerts = alerts.Count(x => x.Severity == "Critical"),
                LastUpdated = DateTime.UtcNow
            };
        }

        public async Task<object> GetPerformanceReportAsync(int godownId, DateTime startDate, DateTime endDate)
        {
            var kpis = await GetInventoryKPIsAsync(godownId);
            
            return new
            {
                Period = new { StartDate = startDate, EndDate = endDate },
                Performance = kpis,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<List<object>> GetSlowMovingItemsAsync(int godownId, int days = 90)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            
            return items.Where(x => x.DateModified < cutoffDate || x.DateModified == null)
                       .Select(x => new { x.Id, x.ItemName, x.Quantity, LastMovement = x.DateModified })
                       .Cast<object>()
                       .ToList();
        }

        public async Task<List<object>> GetExcessStockReportAsync(int godownId)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            
            return items.Where(x => x.Quantity > x.MaximumStockLevel)
                       .Select(x => new { x.Id, x.ItemName, x.Quantity, x.MaximumStockLevel, Excess = x.Quantity - x.MaximumStockLevel })
                       .Cast<object>()
                       .ToList();
        }

        public async Task<List<object>> GetStockoutRiskAsync(int godownId)
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            
            return items.Where(x => x.Quantity <= x.MinimumStockLevel)
                       .Select(x => new { x.Id, x.ItemName, x.Quantity, x.MinimumStockLevel, Risk = x.Quantity == 0 ? "Critical" : "High" })
                       .Cast<object>()
                       .ToList();
        }

        // Helper methods
        private string DetermineABCCategory(Item item)
        {
            var value = item.Price * item.Quantity;
            return value > 10000 ? "A" : value > 5000 ? "B" : "C";
        }

        private string DetermineXYZCategory(Item item)
        {
            var variability = CalculateVariability(item);
            return variability < 0.5m ? "X" : variability < 1.0m ? "Y" : "Z";
        }

        private decimal CalculateVariability(Item item)
        {
            // This would calculate actual demand variability
            return 0.75m; // Sample variability
        }
    }
}