using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Persistence
{
    public class SIMSDbContextFactory : IDesignTimeDbContextFactory<SIMSDbContext>
    {
        public SIMSDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true
                )
                .Build();

            var connectionString = configuration.GetConnectionString("InventoryConnectionString")
                ?? throw new InvalidOperationException("Connection string 'InventoryConnectionString' not found in appsettings.json");

            var builder = new DbContextOptionsBuilder<SIMSDbContext>();
            builder.UseSqlServer(connectionString);

            return new SIMSDbContext(builder.Options);
        }
    }
}