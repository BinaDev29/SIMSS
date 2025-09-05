using Application.CQRS.GodownInventory.Commands.CreateGodownInventory;
using Application.CQRS.GodownInventory.Commands.DeleteGodownInventory;
using Application.CQRS.GodownInventory.Commands.UpdateGodownInventory;
using Application.CQRS.GodownInventory.Queries.GetGodownInventoryById;
using Application.CQRS.GodownInventory.Queries.GetGodownInventoryList;
using Application.DTOs.GodownInventory;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GodownInventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GodownInventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all godown inventory records
        /// </summary>
        /// <returns>List of godown inventory records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetGodownInventories()
        {
            try
            {
                var query = new GetGodownInventoryListQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving godown inventory records.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get godown inventory by ID
        /// </summary>
        /// <param name="id">Godown inventory ID</param>
        /// <returns>Godown inventory details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GodownInventoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GodownInventoryDto>> GetGodownInventory(int id)
        {
            try
            {
                var query = new GetGodownInventoryByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                
                if (result == null)
                {
                    return NotFound(new { message = $"Godown inventory with ID {id} not found." });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving the godown inventory.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new godown inventory record
        /// </summary>
        /// <param name="createGodownInventoryDto">Godown inventory creation data</param>
        /// <returns>Creation result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> CreateGodownInventory([FromBody] CreateGodownInventoryDto createGodownInventoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new CreateGodownInventoryCommand { GodownInventoryDto = createGodownInventoryDto };
                var result = await _mediator.Send(command);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                
                return CreatedAtAction(nameof(GetGodownInventory), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while creating the godown inventory.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing godown inventory record
        /// </summary>
        /// <param name="id">Godown inventory ID</param>
        /// <param name="updateGodownInventoryDto">Godown inventory update data</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateGodownInventory(int id, [FromBody] UpdateGodownInventoryDto updateGodownInventoryDto)
        {
            try
            {
                if (id != updateGodownInventoryDto.Id)
                {
                    return BadRequest(new { message = "ID mismatch between route and body." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new UpdateGodownInventoryCommand { GodownInventoryDto = updateGodownInventoryDto };
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
                    new { message = "An error occurred while updating the godown inventory.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a godown inventory record
        /// </summary>
        /// <param name="id">Godown inventory ID</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteGodownInventory(int id)
        {
            try
            {
                var command = new DeleteGodownInventoryCommand { Id = id };
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
                    new { message = "An error occurred while deleting the godown inventory.", error = ex.Message });
            }
        }
    }
}