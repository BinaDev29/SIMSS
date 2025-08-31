// Persistence/Repositories/NotificationRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<Notification>> GetPagedNotificationsAsync(int pageNumber, int pageSize, int? userId, bool? isRead, CancellationToken cancellationToken)
        {
            var query = _context.Set<Notification>().AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(n => n.UserId == userId.Value);
            }

            if (isRead.HasValue)
            {
                query = query.Where(n => n.IsRead == isRead.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Notification>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<Notification>> GetUnreadNotificationsByUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Set<Notification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUnreadCountByUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Set<Notification>()
                .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
        }

        public async Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken)
        {
            var notification = await _context.Set<Notification>().FindAsync(new object[] { notificationId }, cancellationToken);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
        }

        public async Task MarkAllAsReadByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var notifications = await _context.Set<Notification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
        }
    }
}