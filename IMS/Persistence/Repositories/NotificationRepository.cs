// Persistence/Repositories/NotificationRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly SIMSDbContext _context;

        public NotificationRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<Notification>> GetNotificationsByUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Notification>> GetUnreadNotificationsByUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUnreadCountByUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
        }

        public async Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications.FindAsync(new object[] { notificationId }, cancellationToken);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<PagedResult<Notification>> GetPagedNotificationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Notification>()
                .Include(n => n.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(n => n.Title.Contains(searchTerm) || 
                                        n.Message.Contains(searchTerm) ||
                                        n.Type.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(n => n.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Notification>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<Notification>> INotificationRepository.GetPagedNotificationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}