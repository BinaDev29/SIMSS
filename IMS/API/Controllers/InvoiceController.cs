using Application.CQRS.Invoices.Commands.CreateInvoice;
using Application.CQRS.Invoices.Commands.DeleteInvoice;
using Application.CQRS.Invoices.Commands.UpdateInvoice;
using Application.CQRS.Invoices.Queries.GetInvoiceById;
using Application.CQRS.Invoices.Queries.GetInvoiceList;
using Application.DTOs.Invoice;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetInvoiceListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InvoiceDto>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetInvoiceByIdQuery { Id = id }, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateInvoiceDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateInvoiceCommand { InvoiceDto = dto };
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
        public async Task<ActionResult> Update(int id, [FromBody] UpdateInvoiceDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateInvoiceCommand { Id = id, InvoiceDto = dto };
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
            var command = new DeleteInvoiceCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}