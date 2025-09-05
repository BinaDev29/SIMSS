// API/Controllers/InventoryAlertController.cs
using Application.CQRS.InventoryAlerts.Commands.CreateInventoryAlert;
using Application.CQRS.InventoryAlerts.Queries.GetInventoryAlertList;
using Application.DTOs.InventoryAlert;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryAlertController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryAlertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all inventory alerts
        /// </summary>
        /// <returns>List of inventory alerts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetInventoryAlerts()
        {
            try
            {
                var query = new GetInventoryAlertListQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving inventory alerts.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new inventory alert
        /// </summary>
        /// <param name="createInventoryAlertDto">Inventory alert creation data</param>
        /// <returns>Creation result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> CreateInventoryAlert([FromBody] CreateInventoryAlertDto createInventoryAlertDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // **1. የተስተካከለው ኮድ:** 'CreateInventoryAlertCommand' አሁን የ'InventoryAlert' propertyን ይጠቀማል
                var command = new CreateInventoryAlertCommand { InventoryAlert = createInventoryAlertDto };

                // **2. የተስተካከለው ኮድ:** _mediator.Send() አሁን int (የተፈጠረው alert ID) ይመልሳል
                var newAlertId = await _mediator.Send(command);

                var response = new BaseCommandResponse
                {
                    Id = newAlertId,
                    Success = true,
                    Message = "Inventory alert created successfully."
                };

                return CreatedAtAction(nameof(GetInventoryAlerts), new { id = newAlertId }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating the inventory alert.", error = ex.Message });
            }
        }
    }
}