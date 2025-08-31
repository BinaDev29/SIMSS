// Application/DTOs/DemandForecast/DemandForecastDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.DemandForecast
{
    public class DemandForecastDto : BaseDto
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int GodownId { get; set; }
        public string? GodownName { get; set; }
        public DateTime ForecastDate { get; set; }
        public string ForecastPeriod { get; set; } = string.Empty;
        public int PeriodLength { get; set; }
        public decimal PredictedDemand { get; set; }
        public decimal ConfidenceInterval { get; set; }
        public decimal AccuracyScore { get; set; }
        public decimal BaselineDemand { get; set; }
        public decimal TrendComponent { get; set; }
        public decimal SeasonalComponent { get; set; }
        public decimal PromotionalImpact { get; set; }
        public decimal ExternalFactors { get; set; }
        public decimal ModelAccuracy { get; set; }
        public string ForecastModel { get; set; } = string.Empty;
        public string? ModelParameters { get; set; }
        public DateTime? ActualDate { get; set; }
        public decimal? ActualDemand { get; set; }
        public decimal? ForecastError { get; set; }
    }
}