using Application.CQRS.Items.Commands.CreateItem;
using Application.CQRS.Items.Commands.DeleteItem;
using Application.CQRS.Items.Commands.UpdateItem;
using Application.CQRS.Items.Queries.GetItemById;
using Application.CQRS.Items.Queries.GetItemList;
using Application.DTOs.Item;
using Application.DTOs.Common;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems([FromQuery] ItemQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var query = new GetItemListQuery { Parameters = parameters };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ItemDto>> GetItem(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid item ID.");
            }

            var result = await _mediator.Send(new GetItemByIdQuery { Id = id }, cancellationToken);

            if (result == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseCommandResponse>> CreateItem([FromBody] CreateItemDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                return BadRequest("Item data is required.");
            }

            var command = new CreateItemCommand { ItemDto = dto };
            var response = await _mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetItem), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateItem(int id, [FromBody] UpdateItemDto dto, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid item ID.");
            }

            if (dto == null)
            {
                return BadRequest("Item data is required.");
            }

            var command = new UpdateItemCommand { Id = id, ItemDto = dto };
            var response = await _mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return response.Id == 0 ? NotFound(response) : BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid item ID.");
            }

            var command = new DeleteItemCommand { Id = id };
            var response = await _mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            return NoContent();
        }
    }
}