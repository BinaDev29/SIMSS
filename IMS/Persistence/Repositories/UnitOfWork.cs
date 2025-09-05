// Persistence/Repositories/UnitOfWork.cs
using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SIMSDbContext _dbContext;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;

        public UnitOfWork(SIMSDbContext dbContext)
        {
            _dbContext = dbContext;

            // Core Business Repositories
            CustomerRepository = new CustomerRepository(dbContext);
            DeliveryRepository = new DeliveryRepository(dbContext);
            DeliveryDetailRepository = new DeliveryDetailRepository(dbContext);
            EmployeeRepository = new EmployeeRepository(dbContext);
            GodownInventoryRepository = new GodownInventoryRepository(dbContext);
            GodownRepository = new GodownRepository(dbContext);
            InvoiceDetailRepository = new InvoiceDetailRepository(dbContext);
            InvoiceRepository = new InvoiceRepository(dbContext);
            InwardTransactionRepository = new InwardTransactionRepository(dbContext);
            ItemRepository = new ItemRepository(dbContext);
            OutwardTransactionRepository = new OutwardTransactionRepository(dbContext);
            ReturnTransactionRepository = new ReturnTransactionRepository(dbContext);
            SupplierRepository = new SupplierRepository(dbContext);
            UserRepository = new UserRepository(dbContext);

            // Enhanced Feature Repositories
            InventoryReportRepository = new InventoryReportRepository(dbContext);
            NotificationRepository = new NotificationRepository(dbContext);
            //AuditLogRepository = new AuditLogRepository(dbContext);
            AlertRuleRepository = new AlertRuleRepository(dbContext);
            BatchOperationRepository = new BatchOperationRepository(dbContext);

            // Smart Inventory Management Repositories
            InventoryAlertRepository = new InventoryAlertRepository(dbContext);
            InventoryAnalyticsRepository = new InventoryAnalyticsRepository(dbContext);
            DemandForecastRepository = new DemandForecastRepository(dbContext);
            SmartReorderRepository = new SmartReorderRepository(dbContext);
        }

        // Repositories
        public ICustomerRepository CustomerRepository { get; }
        public IDeliveryRepository DeliveryRepository { get; }
        public IDeliveryDetailRepository DeliveryDetailRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IGodownInventoryRepository GodownInventoryRepository { get; }
        public IGodownRepository GodownRepository { get; }
        public IInvoiceDetailRepository InvoiceDetailRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInwardTransactionRepository InwardTransactionRepository { get; }
        public IItemRepository ItemRepository { get; }
        public IOutwardTransactionRepository OutwardTransactionRepository { get; }
        public IReturnTransactionRepository ReturnTransactionRepository { get; }
        public ISupplierRepository SupplierRepository { get; }
        public IUserRepository UserRepository { get; }
        public IInventoryReportRepository InventoryReportRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IAlertRuleRepository AlertRuleRepository { get; }
        public IBatchOperationRepository BatchOperationRepository { get; }
        public IInventoryAlertRepository InventoryAlertRepository { get; }
        public IInventoryAnalyticsRepository InventoryAnalyticsRepository { get; }
        public IDemandForecastRepository DemandForecastRepository { get; }
        public ISmartReorderRepository SmartReorderRepository { get; }

        // Transaction and Save Methods
        public async Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        Task<Application.Contracts.IDbContextTransaction> IUnitOfWork.BeginTransactionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}