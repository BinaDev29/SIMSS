// DemandForecast.cs - AI-powered demand forecasting
using Domain.Common;
using System;

namespace Domain.Models
{
    public class DemandForecast : BaseDomainEntity
    {
        public required int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public required int GodownId { get; set; }
        public virtual Godown? Godown { get; set; }
        
        public required DateTime ForecastDate { get; set; }
        public required string ForecastPeriod { get; set; } // DAILY, WEEKLY, MONTHLY, QUARTERLY
        public required int PeriodLength { get; set; } // Number of periods ahead
        
        public required decimal PredictedDemand { get; set; }
        public decimal ConfidenceInterval { get; set; }
        public decimal AccuracyScore { get; set; }
        
        // Forecast components
        public decimal BaselineDemand { get; set; }
        public decimal TrendComponent { get; set; }
        public decimal SeasonalComponent { get; set; }
        public decimal PromotionalImpact { get; set; }
        public decimal ExternalFactors { get; set; }
        
        public decimal ModelAccuracy { get; set; }
        public required string ForecastModel { get; set; } // LINEAR, EXPONENTIAL, ARIMA, ML
        public string? ModelParameters { get; set; }
        
        public DateTime? ActualDate { get; set; }
        public decimal? ActualDemand { get; set; }
        public decimal? ForecastError { get; set; }
    }
}