// Persistence/Repositories/InwardTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Persistence;
using System.Threading.Tasks;
using System.Threading;

namespace Persistence.Repositories
{
    public class InwardTransactionRepository(SIMSDbContext dbContext) : GenericRepository<InwardTransaction>(dbContext), IInwardTransactionRepository
    {
    }
}