//// Persistence/Repositories/AuditLogRepository.cs
//using Application.Contracts;
//using Application.DTOs.Common;
//using Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Persistence.Repositories
//{
//    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
//    {
//        private readonly SIMSDbContext _context;

//        public AuditLogRepository(SIMSDbContext dbContext) : base(dbContext)
//        {
//            _context = dbContext;
//        }

//        public async Task<IReadOnlyList<AuditLog>> GetLogsByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken)
//        {
//            return await _context.AuditLogs
//                .Where(al => al.EntityName == entityName && al.EntityId == entityId)
//                .OrderByDescending(al => al.Timestamp)
//                .ToListAsync(cancellationToken);
//        }

//        public async Task<IReadOnlyList<AuditLog>> GetLogsByUserAsync(string username, CancellationToken cancellationToken)
//        {
//            return await _context.AuditLogs
//                .Where(al => al.UserName == username)
//                .OrderByDescending(al => al.Timestamp)
//                .ToListAsync(cancellationToken);
//        }

//        public async Task<IReadOnlyList<AuditLog>> GetLogsByActionAsync(string action, CancellationToken cancellationToken)
//        {
//            return await _context.AuditLogs
//                .Where(al => al.Action == action)
//                .OrderByDescending(al => al.Timestamp)
//                .ToListAsync(cancellationToken);
//        }

//        public async Task<IReadOnlyList<AuditLog>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
//        {
//            return await _context.AuditLogs
//                .Where(al => al.Timestamp >= fromDate && al.Timestamp <= toDate)
//                .OrderByDescending(al => al.Timestamp)
//                .ToListAsync(cancellationToken);
//        }

//        // የGetPagedLogsAsync implementation በGenericRepository ውስጥ አለ፣ ስለዚህ እዚህ አያስፈልግም
//    }
//}