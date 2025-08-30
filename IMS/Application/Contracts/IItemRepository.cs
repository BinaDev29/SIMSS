// Application/Contracts/IItemRepository.cs
using Domain.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Contracts
{
    public interface IItemRepository : IGenericRepository<Item>
    {
    }
}