// Application/Contracts/IInwardTransactionRepository.cs
using Domain.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Contracts
{
    public interface IInwardTransactionRepository : IGenericRepository<InwardTransaction>
    {
    }
}