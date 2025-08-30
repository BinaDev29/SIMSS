using Application.CQRS.Items.Commands.CreateItem;
using Application.CQRS.Items.Commands.DeleteItem;
using Application.CQRS.Items.Commands.UpdateItem;
using Application.CQRS.Items.Queries.GetItemById;
using Application.CQRS.Items.Queries.GetItemList;
using Application.DTOs.Item;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetItemListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemDto>> GetItem(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetItemByIdQuery { Id = id }, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> CreateItem([FromBody] CreateItemDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateItemCommand { ItemDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetItem), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateItem(int id, [FromBody] UpdateItemDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateItemCommand { Id = id, ItemDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteItem(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteItemCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}