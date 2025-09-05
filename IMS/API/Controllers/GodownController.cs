using Application.CQRS.Godown.Commands.CreateGodown;
using Application.CQRS.Godown.Commands.DeleteGodown;
using Application.CQRS.Godown.Commands.UpdateGodown;
using Application.CQRS.Godown.Queries.GetGodownById;
using Application.CQRS.Godown.Queries.GetGodownList;
using Application.DTOs.Godown;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GodownController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GodownController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all godowns/warehouses
        /// </summary>
        /// <returns>List of godowns</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGodowns()
        {
            try
            {
                var query = new GetGodownListQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving godowns.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get godown by ID
        /// </summary>
        /// <param name="id">Godown ID</param>
        /// <returns>Godown details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GodownDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GodownDto>> GetGodown(int id)
        {
            try
            {
                var query = new GetGodownByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                
                if (result == null)
                {
                    return NotFound(new { message = $"Godown with ID {id} not found." });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving the godown.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new godown
        /// </summary>
        /// <param name="createGodownDto">Godown creation data</param>
        /// <returns>Creation result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> CreateGodown([FromBody] CreateGodownDto createGodownDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new CreateGodownCommand { GodownDto = createGodownDto };
                var result = await _mediator.Send(command);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                
                return CreatedAtAction(nameof(GetGodown), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while creating the godown.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing godown
        /// </summary>
        /// <param name="id">Godown ID</param>
        /// <param name="updateGodownDto">Godown update data</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateGodown(int id, [FromBody] UpdateGodownDto updateGodownDto)
        {
            try
            {
                if (id != updateGodownDto.Id)
                {
                    return BadRequest(new { message = "ID mismatch between route and body." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new UpdateGodownCommand { GodownDto = updateGodownDto };
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
                    new { message = "An error occurred while updating the godown.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a godown
        /// </summary>
        /// <param name="id">Godown ID</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteGodown(int id)
        {
            try
            {
                var command = new DeleteGodownCommand { Id = id };
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
                    new { message = "An error occurred while deleting the godown.", error = ex.Message });
            }
        }
    }
}