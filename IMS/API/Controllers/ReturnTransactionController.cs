using Application.CQRS.ReturnTransactions.Commands.UpdateReturnTransaction;
using Application.CQRS.Transactions.Commands.CreateReturnTransaction;
using Application.CQRS.Transactions.Commands.DeleteReturnTransaction;
using Application.CQRS.Transactions.Queries.GetReturnTransactionById;
using Application.CQRS.Transactions.Queries.GetReturnTransactionList;
using Application.DTOs.Transaction;
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
    public class ReturnTransactionController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReturnTransactionDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetReturnTransactionListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnTransactionDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetReturnTransactionByIdQuery { Id = id }, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateReturnTransactionDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateReturnTransactionCommand { ReturnTransactionDto = dto };
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateReturnTransactionDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateReturnTransactionCommand { Id = id, ReturnTransactionDto = dto };
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
            {
                if (response.Errors?.Count > 0)
                {
                    return BadRequest(response);
                }
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteReturnTransactionCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return NoContent();
        }
    }
}