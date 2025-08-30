// Persistence/Repositories/ReturnTransactionRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ReturnTransactionRepository(SIMSDbContext dbContext)
        : GenericRepository<ReturnTransaction>(dbContext), IReturnTransactionRepository
    {
        // This constructor is simplified by using a primary constructor
        // public ReturnTransactionRepository(SIMSDbContext dbContext) : base(dbContext) { }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}