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
            // Register DbContext
            services.AddDbContext<SIMSDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("InventoryConnectionString")));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Core Business Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IGodownRepository, GodownRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IInwardTransactionRepository, InwardTransactionRepository>();
            services.AddScoped<IOutwardTransactionRepository, OutwardTransactionRepository>();
            services.AddScoped<IReturnTransactionRepository, ReturnTransactionRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IDeliveryDetailRepository, DeliveryDetailRepository>();
            services.AddScoped<IGodownInventoryRepository, GodownInventoryRepository>();

            // Register Enhanced Feature Repositories
            services.AddScoped<IInventoryReportRepository, InventoryReportRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IAlertRuleRepository, AlertRuleRepository>();
            services.AddScoped<IBatchOperationRepository, BatchOperationRepository>();

            // Register Smart Inventory Management Repositories
            services.AddScoped<IInventoryAlertRepository, InventoryAlertRepository>();
            services.AddScoped<IInventoryAnalyticsRepository, InventoryAnalyticsRepository>();
            services.AddScoped<IDemandForecastRepository, DemandForecastRepository>();
            services.AddScoped<ISmartReorderRepository, SmartReorderRepository>();

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}