// Persistence/Repositories/ItemRepository.cs
using Application.Contracts;
using Domain.Models;
using Persistence;
using System.Threading.Tasks;
using System.Threading;

namespace Persistence.Repositories
{
    public class ItemRepository(SIMSDbContext dbContext) : GenericRepository<Item>(dbContext), IItemRepository
    {
    }
}