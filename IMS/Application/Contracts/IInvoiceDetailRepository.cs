// Application/Contracts/IInvoiceDetailRepository.cs
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInvoiceDetailRepository : IGenericRepository<InvoiceDetail>
    {
        Task<List<InvoiceDetail>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken);
        Task DeleteRangeAsync(ICollection<InvoiceDetail> invoiceDetails, CancellationToken cancellationToken);
    }
}