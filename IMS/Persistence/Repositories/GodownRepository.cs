// Persistence/Repositories/GodownRepository.cs
using Application.Contracts;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class GodownRepository(SIMSDbContext dbContext) : GenericRepository<Godown>(dbContext), IGodownRepository
    {
    }
}