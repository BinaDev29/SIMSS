using Application.CQRS.AlertRule.Commands.CreateAlertRule;
using Application.CQRS.AlertRule.Commands.DeleteAlertRule;
using Application.CQRS.AlertRule.Commands.UpdateAlertRule;
using Application.CQRS.AlertRule.Queries.GetAlertRuleById;
using Application.CQRS.AlertRule.Queries.GetAlertRules;
using Application.DTOs.AlertRule;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertRuleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlertRuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all alert rules
        /// </summary>
        /// <returns>List of alert rules</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<AlertRuleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyList<AlertRuleDto>>> GetAlertRules()
        {
            try
            {
                var query = new GetAlertRulesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving alert rules.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get alert rule by ID
        /// </summary>
        /// <param name="id">Alert rule ID</param>
        /// <returns>Alert rule details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlertRuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AlertRuleDto>> GetAlertRule(int id)
        {
            try
            {
                var query = new GetAlertRuleByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                
                if (result == null)
                {
                    return NotFound(new { message = $"Alert rule with ID {id} not found." });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving the alert rule.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new alert rule
        /// </summary>
        /// <param name="createAlertRuleDto">Alert rule creation data</param>
        /// <returns>Creation result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> CreateAlertRule([FromBody] CreateAlertRuleDto createAlertRuleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new CreateAlertRuleCommand { AlertRuleDto = createAlertRuleDto };
                var result = await _mediator.Send(command);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                
                return CreatedAtAction(nameof(GetAlertRule), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while creating the alert rule.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing alert rule
        /// </summary>
        /// <param name="id">Alert rule ID</param>
        /// <param name="updateAlertRuleDto">Alert rule update data</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateAlertRule(int id, [FromBody] UpdateAlertRuleDto updateAlertRuleDto)
        {
            try
            {
                if (id != updateAlertRuleDto.Id)
                {
                    return BadRequest(new { message = "ID mismatch between route and body." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new UpdateAlertRuleCommand { AlertRuleDto = updateAlertRuleDto };
                var result = await _mediator.Send(command);
                
                if (!result.Success)
                {
                    if (result.Errors.Any(e => e.Contains("not found")))
                    {
                        return NotFound(result);
                    }
                    return BadRequest(result);
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while updating the alert rule.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an alert rule
        /// </summary>
        /// <param name="id">Alert rule ID</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteAlertRule(int id)
        {
            try
            {
                var command = new DeleteAlertRuleCommand { Id = id };
                var result = await _mediator.Send(command);
                
                if (!result.Success)
                {
                    if (result.Errors.Any(e => e.Contains("not found")))
                    {
                        return NotFound(result);
                    }
                    return BadRequest(result);
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while deleting the alert rule.", error = ex.Message });
            }
        }
    }
}