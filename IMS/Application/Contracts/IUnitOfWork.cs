// Application/Contracts/IUnitOfWork.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        // Core Business Repositories
        ICustomerRepository CustomerRepository { get; }
        IDeliveryRepository DeliveryRepository { get; }
        IDeliveryDetailRepository DeliveryDetailRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IGodownInventoryRepository GodownInventoryRepository { get; }
        IGodownRepository GodownRepository { get; }
        IInvoiceDetailRepository InvoiceDetailRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInwardTransactionRepository InwardTransactionRepository { get; }
        IItemRepository ItemRepository { get; }
        IOutwardTransactionRepository OutwardTransactionRepository { get; }
        IReturnTransactionRepository ReturnTransactionRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IUserRepository UserRepository { get; }

        // Enhanced Feature Repositories
        IInventoryReportRepository InventoryReportRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IAlertRuleRepository AlertRuleRepository { get; }
        IBatchOperationRepository BatchOperationRepository { get; }

        // Smart Inventory Management Repositories
        IInventoryAlertRepository InventoryAlertRepository { get; }
        IInventoryAnalyticsRepository InventoryAnalyticsRepository { get; }
        IDemandForecastRepository DemandForecastRepository { get; }
        ISmartReorderRepository SmartReorderRepository { get; }

        // Transaction and Save Methods
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}