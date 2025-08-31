using Application.CQRS.Customers.Commands.CreateCustomer;
using Application.CQRS.Customers.Commands.DeleteCustomer;
using Application.CQRS.Customers.Commands.UpdateCustomer;
using Application.CQRS.Customers.Queries.GetCustomerById;
using Application.CQRS.Customers.Queries.GetCustomerList;
using Application.DTOs.Customer;
using Application.DTOs.Common;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(IMediator mediator) : ControllerBase
    {
        // GET: api/customers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll([FromQuery] CustomerQueryParameters parameters, CancellationToken cancellationToken)
        {
            var query = new GetCustomerListQuery { Parameters = parameters };
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetCustomerByIdQuery { Id = id }, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/customers
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand { CustomerDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        // PUT: api/customers
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseCommandResponse>> Update([FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerCommand { CustomerDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                // The handler's response determines if it's a NotFound or BadRequest.
                // We'll assume the handler returns a specific message for "not found".
                return NotFound(response);
            }

            return Ok(response);
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}