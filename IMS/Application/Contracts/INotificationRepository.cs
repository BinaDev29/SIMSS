// Application/Contracts/INotificationRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<PagedResult<Notification>> GetPagedNotificationsAsync(int pageNumber, int pageSize, int? userId, bool? isRead, CancellationToken cancellationToken);
        Task<IReadOnlyList<Notification>> GetUnreadNotificationsByUserAsync(int userId, CancellationToken cancellationToken);
        Task<int> GetUnreadCountByUserAsync(int userId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken);
        Task MarkAllAsReadByUserAsync(int userId, CancellationToken cancellationToken);
    }
}