using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SIMSDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("InventoryConnectionString")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IGodownRepository, GodownRepository>();
            services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInwardTransactionRepository, InwardTransactionRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOutwardTransactionRepository, OutwardTransactionRepository>();
            services.AddScoped<IReturnTransactionRepository, ReturnTransactionRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IDeliveryDetailRepository, DeliveryDetailRepository>();
            services.AddScoped<IGodownInventoryRepository, GodownInventoryRepository>();

            return services;
        }
    }
}