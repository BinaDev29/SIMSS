using Application.Contracts;
using Application.Services;
using Application.DTOs.Notifications;
using Domain.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService(
        IGenericRepository<Notification> notificationRepository, 
        IGenericRepository<Item> itemRepository,
        IMapper mapper) : INotificationService
    {
        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createDto)
        {
            var notification = new Notification
            {
                Title = createDto.Title,
                Message = createDto.Message,
                Type = createDto.Type,
                Priority = GetPriorityValue(createDto.Priority), // Convert string to int
                UserId = createDto.UserId,
                IsRead = false
            };

            var createdNotification = await notificationRepository.AddAsync(notification, CancellationToken.None);
            return mapper.Map<NotificationDto>(createdNotification);
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            var allNotifications = await notificationRepository.GetAllAsync(CancellationToken.None);
            var userNotifications = allNotifications.Where(x => x.UserId == userId);
            
            if (unreadOnly)
                userNotifications = userNotifications.Where(x => !x.IsRead);

            return mapper.Map<IEnumerable<NotificationDto>>(userNotifications.OrderByDescending(x => x.CreatedDate));
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await notificationRepository.GetByIdAsync(notificationId, CancellationToken.None);
            if (notification != null && notification.UserId == userId)
            {
                notification.IsRead = true;
                await notificationRepository.Update(notification, CancellationToken.None);
                return true;
            }
            return false;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await GetUserNotificationsAsync(userId, true);
            foreach (var notificationDto in notifications)
            {
                var notification = await notificationRepository.GetByIdAsync(notificationDto.Id, CancellationToken.None);
                if (notification != null)
                {
                    notification.IsRead = true;
                    await notificationRepository.Update(notification, CancellationToken.None);
                }
            }
            return true;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var unreadNotifications = await GetUserNotificationsAsync(userId, true);
            return unreadNotifications.Count();
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId, int userId)
        {
            var notification = await notificationRepository.GetByIdAsync(notificationId, CancellationToken.None);
            if (notification != null && notification.UserId == userId)
            {
                await notificationRepository.Delete(notification, CancellationToken.None);
                return true;
            }
            return false;
        }

        public async Task SendLowStockAlertsAsync()
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var lowStockItems = items.Where(x => x.Quantity <= x.MinimumStockLevel).ToList();

            foreach (var item in lowStockItems)
            {
                var notification = new Notification
                {
                    Title = "Low Stock Alert",
                    Message = $"Item '{item.ItemName}' is running low. Current stock: {item.Quantity}, Minimum level: {item.MinimumStockLevel}",
                    Type = "Warning",
                    Priority = 3, // High priority
                    UserId = 1, // System notification - should be sent to admin users
                    IsRead = false
                };

                await notificationRepository.AddAsync(notification, CancellationToken.None);
            }
        }

        public async Task SendExpiryAlertsAsync()
        {
            var items = await itemRepository.GetAllAsync(CancellationToken.None);
            var expiringItems = items.Where(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value <= DateTime.UtcNow.AddDays(30)).ToList();

            foreach (var item in expiringItems)
            {
                var notification = new Notification
                {
                    Title = "Item Expiry Alert",
                    Message = $"Item '{item.ItemName}' is expiring soon. Expiry date: {item.ExpiryDate:yyyy-MM-dd}",
                    Type = "Warning",
                    Priority = 3, // High priority
                    UserId = 1, // System notification - should be sent to admin users
                    IsRead = false
                };

                await notificationRepository.AddAsync(notification, CancellationToken.None);
            }
        }

        private int GetPriorityValue(string priority)
        {
            return priority?.ToLower() switch
            {
                "low" => 1,
                "medium" => 2,
                "high" => 3,
                "critical" => 4,
                _ => 2 // Default to medium
            };
        }
    }
}