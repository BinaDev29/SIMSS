// Application/Contracts/IInventoryReportRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryReportRepository : IGenericRepository<InventoryReport>
    {
        Task<IReadOnlyList<InventoryReport>> GetReportsByTypeAsync(string reportType, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryReport>> GetReportsByUserAsync(string userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryReport>> GetReportsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
        Task<PagedResult<InventoryReport>> GetPagedReportsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}