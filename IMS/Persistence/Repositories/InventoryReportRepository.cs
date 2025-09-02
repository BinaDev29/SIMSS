// Persistence/Repositories/InventoryReportRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryReportRepository : GenericRepository<InventoryReport>, IInventoryReportRepository
    {
        private readonly SIMSDbContext _context;

        public InventoryReportRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<InventoryReport>> GetReportsByTypeAsync(string reportType, CancellationToken cancellationToken)
        {
            return await _context.InventoryReports
                .Where(ir => ir.ReportType == reportType)
                .OrderByDescending(ir => ir.GeneratedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryReport>> GetReportsByUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.InventoryReports
                .Where(ir => ir.GeneratedBy == userId)
                .OrderByDescending(ir => ir.GeneratedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryReport>> GetReportsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            return await _context.InventoryReports
                .Where(ir => ir.FromDate >= fromDate && ir.ToDate <= toDate)
                .OrderByDescending(ir => ir.GeneratedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<InventoryReport>> GetPagedReportsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryReport>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ir => ir.ReportName.Contains(searchTerm) || 
                                         ir.ReportType.Contains(searchTerm) ||
                                         ir.GeneratedBy.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(ir => ir.GeneratedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryReport>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<InventoryReport>> IInventoryReportRepository.GetPagedReportsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}