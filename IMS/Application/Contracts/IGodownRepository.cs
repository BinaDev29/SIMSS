// Application/Contracts/IGodownRepository.cs
using Domain.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Contracts
{
    public interface IGodownRepository : IGenericRepository<Godown>
    {
    }
}