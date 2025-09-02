// Application/Contracts/IInvoiceRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice?> GetInvoiceByNumberAsync(string invoiceNumber, CancellationToken cancellationToken);
        Task<IReadOnlyList<Invoice>> GetInvoicesByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Invoice>> GetUnpaidInvoicesAsync(CancellationToken cancellationToken);
        Task<PagedResult<Invoice>> GetPagedInvoicesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}