// Application/Contracts/INotificationRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetNotificationsByUserAsync(int userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Notification>> GetUnreadNotificationsByUserAsync(int userId, CancellationToken cancellationToken);
        Task<int> GetUnreadCountByUserAsync(int userId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken);
        Task<PagedResult<Notification>> GetPagedNotificationsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}