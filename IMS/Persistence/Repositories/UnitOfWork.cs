// Persistence/Repositories/UnitOfWork.cs
using Application.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SIMSDbContext _dbContext;

        public UnitOfWork(SIMSDbContext dbContext)
        {
            _dbContext = dbContext;
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
        }

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

        public IInventoryReportRepository InventoryReportRepository => throw new NotImplementedException();

        public INotificationRepository NotificationRepository => throw new NotImplementedException();

        public IAuditLogRepository AuditLogRepository => throw new NotImplementedException();

        public IAlertRuleRepository AlertRuleRepository => throw new NotImplementedException();

        public IBatchOperationRepository BatchOperationRepository => throw new NotImplementedException();

        public IInventoryAlertRepository InventoryAlertRepository => throw new NotImplementedException();

        public IInventoryAnalyticsRepository InventoryAnalyticsRepository => throw new NotImplementedException();

        public IDemandForecastRepository DemandForecastRepository => throw new NotImplementedException();

        public ISmartReorderRepository SmartReorderRepository => throw new NotImplementedException();

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}