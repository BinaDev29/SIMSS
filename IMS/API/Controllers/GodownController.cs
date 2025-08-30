using Application.CQRS.Godowns.Commands.CreateGodown;
using Application.CQRS.Godowns.Commands.UpdateGodown;
using Application.CQRS.Godowns.Commands.DeleteGodown;
using Application.CQRS.Godowns.Queries.GetGodownById;
using Application.CQRS.Godowns.Queries.GetGodownList;
using Application.DTOs.Godown;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GodownsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GodownDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetGodownListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GodownDto>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetGodownByIdQuery { Id = id }, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateGodownDto createDto, CancellationToken cancellationToken)
        {
            var command = new CreateGodownCommand { GodownDto = createDto };
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
        public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateGodownDto updateDto, CancellationToken cancellationToken)
        {
            var command = new UpdateGodownCommand { Id = id, GodownDto = updateDto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                // We assume the handler returns a specific response for not found scenarios.
                // The controller can then decide what status code to return.
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new DeleteGodownCommand { Id = id }, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}