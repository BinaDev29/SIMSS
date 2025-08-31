using Application.DTOs.Notifications;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(notificationService);
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUserNotifications(int userId, [FromQuery] bool unreadOnly = false)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var notifications = await _notificationService.GetUserNotificationsAsync(userId, unreadOnly);
            return Ok(notifications);
        }

        [HttpGet("user/{userId:int}/unread-count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(count);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NotificationDto>> CreateNotification([FromBody] CreateNotificationDto createDto)
        {
            if (createDto == null)
            {
                return BadRequest("Notification data is required.");
            }

            if (string.IsNullOrWhiteSpace(createDto.Title) || string.IsNullOrWhiteSpace(createDto.Message))
            {
                return BadRequest("Title and message are required.");
            }

            var notification = await _notificationService.CreateNotificationAsync(createDto);
            return CreatedAtAction(nameof(GetUserNotifications), new { userId = createDto.UserId }, notification);
        }

        [HttpPut("{notificationId:int}/mark-read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> MarkAsRead(int notificationId, [FromQuery] int userId)
        {
            if (notificationId <= 0 || userId <= 0)
            {
                return BadRequest("Invalid notification ID or user ID.");
            }

            var success = await _notificationService.MarkAsReadAsync(notificationId, userId);
            
            if (!success)
            {
                return NotFound("Notification not found or access denied.");
            }

            return Ok();
        }

        [HttpPut("user/{userId:int}/mark-all-read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> MarkAllAsRead(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok();
        }

        [HttpDelete("{notificationId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteNotification(int notificationId, [FromQuery] int userId)
        {
            if (notificationId <= 0 || userId <= 0)
            {
                return BadRequest("Invalid notification ID or user ID.");
            }

            var success = await _notificationService.DeleteNotificationAsync(notificationId, userId);
            
            if (!success)
            {
                return NotFound("Notification not found or access denied.");
            }

            return NoContent();
        }

        [HttpPost("alerts/low-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendLowStockAlerts()
        {
            await _notificationService.SendLowStockAlertsAsync();
            return Ok(new { message = "Low stock alerts sent successfully." });
        }

        [HttpPost("alerts/expiry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendExpiryAlerts()
        {
            await _notificationService.SendExpiryAlertsAsync();
            return Ok(new { message = "Expiry alerts sent successfully." });
        }
    }
}