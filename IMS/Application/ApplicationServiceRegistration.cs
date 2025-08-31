using System.Reflection;
using Application.Contracts;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register application services
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IGodownInventoryService, GodownInventoryService>();
            services.AddScoped<IInventoryAnalyticsService, InventoryAnalyticsService>();

            return services;
        }
    }
}