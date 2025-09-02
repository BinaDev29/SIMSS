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
        Task<IReadOnlyList<InvoiceDetail>> GetDetailsByInvoiceAsync(int invoiceId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InvoiceDetail>> GetDetailsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<PagedResult<InvoiceDetail>> GetPagedDetailsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}