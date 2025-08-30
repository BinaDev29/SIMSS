using Application.Contracts;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(SIMSDbContext dbContext) : base(dbContext) { }
    }
}