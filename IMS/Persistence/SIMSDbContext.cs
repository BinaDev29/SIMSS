using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class SIMSDbContext : DbContext
    {
        public SIMSDbContext(DbContextOptions<SIMSDbContext> options) : base(options)
        {
        }

        // Core Business Entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Godown> Godowns { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<InwardTransaction> InwardTransactions { get; set; } = null!;
        public DbSet<OutwardTransaction> OutwardTransactions { get; set; } = null!;
        public DbSet<ReturnTransaction> ReturnTransactions { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
        public DbSet<Delivery> Deliveries { get; set; } = null!;
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; } = null!;
        public DbSet<GodownInventory> GodownInventories { get; set; } = null!;

        // Enhanced Features - Reporting & Notifications
        public DbSet<InventoryReport> InventoryReports { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<AlertRule> AlertRules { get; set; } = null!;
        public DbSet<BatchOperation> BatchOperations { get; set; } = null!;

        // Smart Inventory Management
        public DbSet<InventoryAlert> InventoryAlerts { get; set; } = null!;
        public DbSet<InventoryAnalytics> InventoryAnalytics { get; set; } = null!;
        public DbSet<DemandForecast> DemandForecasts { get; set; } = null!;
        public DbSet<SmartReorder> SmartReorders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SIMSDbContext).Assembly);
            
            // Configure Notification relationships
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure InventoryAlert relationships
            modelBuilder.Entity<InventoryAlert>(entity =>
            {
                entity.HasOne(ia => ia.Item)
                    .WithMany()
                    .HasForeignKey(ia => ia.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ia => ia.Godown)
                    .WithMany()
                    .HasForeignKey(ia => ia.GodownId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure InventoryAnalytics relationships
            modelBuilder.Entity<InventoryAnalytics>(entity =>
            {
                entity.HasOne(ia => ia.Item)
                    .WithMany()
                    .HasForeignKey(ia => ia.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ia => ia.Godown)
                    .WithMany()
                    .HasForeignKey(ia => ia.GodownId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure DemandForecast relationships
            modelBuilder.Entity<DemandForecast>(entity =>
            {
                entity.HasOne(df => df.Item)
                    .WithMany()
                    .HasForeignKey(df => df.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(df => df.Godown)
                    .WithMany()
                    .HasForeignKey(df => df.GodownId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure SmartReorder relationships
            modelBuilder.Entity<SmartReorder>(entity =>
            {
                entity.HasOne(sr => sr.Item)
                    .WithMany()
                    .HasForeignKey(sr => sr.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sr => sr.Godown)
                    .WithMany()
                    .HasForeignKey(sr => sr.GodownId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sr => sr.Supplier)
                    .WithMany()
                    .HasForeignKey(sr => sr.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure indexes for better performance
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(a => new { a.EntityName, a.EntityId })
                    .HasDatabaseName("IX_AuditLog_Entity");

                entity.HasIndex(a => a.Timestamp)
                    .HasDatabaseName("IX_AuditLog_Timestamp");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(n => new { n.UserId, n.IsRead })
                    .HasDatabaseName("IX_Notification_User_Read");
            });

            modelBuilder.Entity<AlertRule>(entity =>
            {
                entity.HasIndex(a => a.IsActive)
                    .HasDatabaseName("IX_AlertRule_Active");
            });

            modelBuilder.Entity<InventoryAlert>(entity =>
            {
                entity.HasIndex(ia => new { ia.ItemId, ia.GodownId })
                    .HasDatabaseName("IX_InventoryAlert_Item_Godown");

                entity.HasIndex(ia => new { ia.AlertType, ia.IsActive })
                    .HasDatabaseName("IX_InventoryAlert_Type_Active");
            });

            modelBuilder.Entity<InventoryAnalytics>(entity =>
            {
                entity.HasIndex(ia => new { ia.ItemId, ia.GodownId, ia.AnalysisDate })
                    .HasDatabaseName("IX_InventoryAnalytics_Item_Godown_Date");
            });

            modelBuilder.Entity<DemandForecast>(entity =>
            {
                entity.HasIndex(df => new { df.ItemId, df.GodownId, df.ForecastDate })
                    .HasDatabaseName("IX_DemandForecast_Item_Godown_Date");
            });

            modelBuilder.Entity<SmartReorder>(entity =>
            {
                entity.HasIndex(sr => new { sr.ItemId, sr.GodownId })
                    .HasDatabaseName("IX_SmartReorder_Item_Godown");

                entity.HasIndex(sr => sr.Status)
                    .HasDatabaseName("IX_SmartReorder_Status");
            });

            // Set all foreign key relationships to Restrict to prevent cascading deletes
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set audit fields for entities inheriting from BaseDomainEntity
            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        break;
                }
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}