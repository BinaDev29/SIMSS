using Application.DTOs.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createDto);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        Task<bool> MarkAsReadAsync(int notificationId, int userId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> DeleteNotificationAsync(int notificationId, int userId);
        Task SendLowStockAlertsAsync();
        Task SendExpiryAlertsAsync();
    }
}