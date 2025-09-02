using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Persistence
{
    public class SIMSDbContextFactory : IDesignTimeDbContextFactory<SIMSDbContext>
    {
        public SIMSDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SIMSDbContext>();
            var connectionString = configuration.GetConnectionString("InventoryConnectionString");
            
            optionsBuilder.UseSqlServer(connectionString);

            return new SIMSDbContext(optionsBuilder.Options);
        }
    }
}