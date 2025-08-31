// IInventoryAnalyticsService.cs - Smart analytics service interface
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAnalyticsService
    {
        // Demand Forecasting
        Task<DemandForecast> GenerateDemandForecastAsync(int itemId, int godownId, string period, int length);
        Task<List<DemandForecast>> GetForecastsAsync(int? itemId = null, int? godownId = null);
        Task<decimal> CalculateForecastAccuracyAsync(int itemId, int godownId, DateTime startDate, DateTime endDate);
        
        // Smart Alerts
        Task<List<InventoryAlert>> GenerateSmartAlertsAsync();
        Task<List<InventoryAlert>> GetActiveAlertsAsync(int? godownId = null);
        Task<List<InventoryAlert>> GetCriticalAlertsAsync();
        Task ProcessAlertAsync(int alertId, string action, string? notes = null);
        
        // ABC/XYZ Analysis
        Task<List<InventoryAnalytics>> PerformABCAnalysisAsync(int godownId);
        Task<List<InventoryAnalytics>> PerformXYZAnalysisAsync(int godownId);
        Task<List<InventoryAnalytics>> GetCombinedAnalysisAsync(int godownId);
        
        // Smart Reordering
        Task<List<SmartReorder>> GenerateReorderRecommendationsAsync(int? godownId = null);
        Task<SmartReorder> CalculateOptimalReorderAsync(int itemId, int godownId);
        Task<decimal> CalculateEconomicOrderQuantityAsync(int itemId, int godownId);
        Task<decimal> CalculateSafetyStockAsync(int itemId, int godownId);
        
        // Performance Analytics
        Task<Dictionary<string, decimal>> GetInventoryKPIsAsync(int godownId);
        Task<List<InventoryAnalytics>> GetVelocityAnalysisAsync(int godownId);
        Task<decimal> CalculateTurnoverRateAsync(int itemId, int godownId);
        Task<decimal> CalculateServiceLevelAsync(int itemId, int godownId);
        
        // Reporting
        Task<object> GetInventoryDashboardAsync(int godownId);
        Task<object> GetPerformanceReportAsync(int godownId, DateTime startDate, DateTime endDate);
        Task<List<object>> GetSlowMovingItemsAsync(int godownId, int days = 90);
        Task<List<object>> GetExcessStockReportAsync(int godownId);
        Task<List<object>> GetStockoutRiskAsync(int godownId);
    }
}