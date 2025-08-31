// Application/Contracts/IInvoiceDetailRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInvoiceDetailRepository : IGenericRepository<InvoiceDetail>
    {
        Task<bool> HasDetailsByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<PagedResult<InvoiceDetail>> GetPagedInvoiceDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<IReadOnlyList<InvoiceDetail>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken);
        Task DeleteRangeAsync(IEnumerable<InvoiceDetail> entities, CancellationToken cancellationToken);
    }
}