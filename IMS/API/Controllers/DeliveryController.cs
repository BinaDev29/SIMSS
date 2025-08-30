using Application.CQRS.Deliveries.Commands.CreateDelivery;
using Application.CQRS.Deliveries.Commands.DeleteDelivery;
using Application.CQRS.Deliveries.Commands.UpdateDelivery;
using Application.CQRS.Deliveries.Queries.GetDeliveryById;
using Application.CQRS.Deliveries.Queries.GetDeliveryList;
using Application.DTOs.Delivery;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDeliveryListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeliveryDto>> Get(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDeliveryByIdQuery { Id = id }, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateDeliveryDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateDeliveryCommand { DeliveryDto = dto };
        var response = await mediator.Send(command, cancellationToken);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateDeliveryDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateDeliveryCommand { Id = id, DeliveryDto = dto };
        var response = await mediator.Send(command, cancellationToken);

        if (!response.Success)
        {
            // For simplicity and robustness, we assume if the operation failed and it's not a BadRequest, it's a NotFound.
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteDeliveryCommand { Id = id };
        var response = await mediator.Send(command, cancellationToken);

        if (!response.Success)
        {
            return NotFound();
        }

        return NoContent();
    }
}