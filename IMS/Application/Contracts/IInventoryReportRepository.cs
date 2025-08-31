// Application/Contracts/IInventoryReportRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryReportRepository : IGenericRepository<InventoryReport>
    {
        Task<PagedResult<InventoryReport>> GetPagedReportsAsync(int pageNumber, int pageSize, string? reportType, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryReport>> GetReportsByTypeAsync(string reportType, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryReport>> GetReportsByStatusAsync(string status, CancellationToken cancellationToken);
    }
}