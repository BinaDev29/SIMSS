using Application.CQRS.InwardTransactions.Commands.UpdateInwardTransaction;
using Application.CQRS.Transactions.Commands.CreateInwardTransaction;
using Application.CQRS.Transactions.Commands.DeleteInwardTransaction;
using Application.CQRS.Transactions.Queries.GetInwardTransactionById;
using Application.CQRS.Transactions.Queries.GetInwardTransactionList;
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
    public class InwardTransactionController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InwardTransactionDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetInwardTransactionListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InwardTransactionDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetInwardTransactionByIdQuery { Id = id }, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateInwardTransactionDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateInwardTransactionCommand { InwardTransactionDto = dto };
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
        public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateInwardTransactionDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateInwardTransactionCommand { Id = id, InwardTransactionDto = dto };
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
            var command = new DeleteInwardTransactionCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return NoContent();
        }
    }
}