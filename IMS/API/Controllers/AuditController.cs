using Application.DTOs.Audit;
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
    [Authorize(Roles = "Admin,Manager")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            ArgumentNullException.ThrowIfNull(auditService);
            _auditService = auditService;
        }

        [HttpGet("logs")]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogs(
            [FromQuery] string? entityName = null,
            [FromQuery] string? entityId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid page number or page size. Page size must be between 1 and 100.");
            }

            var logs = await _auditService.GetAuditLogsAsync(entityName, entityId, pageNumber, pageSize);
            return Ok(logs);
        }

        [HttpGet("user/{userId}/activity")]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetUserActivity(
            string userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid page number or page size. Page size must be between 1 and 100.");
            }

            var activity = await _auditService.GetUserActivityAsync(userId, pageNumber, pageSize);
            return Ok(activity);
        }

        [HttpGet("summary")]
        [ProducesResponseType(typeof(AuditSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AuditSummaryDto>> GetAuditSummary(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return BadRequest("From date cannot be greater than to date.");
            }

            if (toDate > DateTime.UtcNow)
            {
                return BadRequest("To date cannot be in the future.");
            }

            var summary = await _auditService.GetAuditSummaryAsync(fromDate, toDate);
            return Ok(summary);
        }
    }
}