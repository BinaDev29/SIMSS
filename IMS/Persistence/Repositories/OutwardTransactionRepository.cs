using Application.Contracts;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class OutwardTransactionRepository : GenericRepository<OutwardTransaction>, IOutwardTransactionRepository
    {
        public OutwardTransactionRepository(SIMSDbContext dbContext) : base(dbContext) { }
    }
}