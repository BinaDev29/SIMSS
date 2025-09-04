// Application/Services/IReportingService.cs
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IReportingService
    {
        Task<object> GenerateInventoryReportAsync(CancellationToken cancellationToken = default);
        Task<object> GenerateSalesReportAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<object> GenerateCustomerReportAsync(CancellationToken cancellationToken = default);
    }
}