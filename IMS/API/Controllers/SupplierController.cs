using Application.CQRS.Suppliers.Commands.CreateSupplier;
using Application.CQRS.Suppliers.Commands.UpdateSupplier;
using Application.CQRS.Suppliers.Commands.DeleteSupplier;
using Application.CQRS.Suppliers.Queries.GetSupplierById;
using Application.CQRS.Suppliers.Queries.GetSupplierList;
using Application.DTOs.Supplier;
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
    public class SupplierController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSupplierListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetSupplierByIdQuery { Id = id }, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateSupplierDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateSupplierCommand { SupplierDto = dto };
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
        public async Task<ActionResult> Update(int id, [FromBody] UpdateSupplierDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateSupplierCommand { Id = id, SupplierDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                // Return NotFound for non-existent entities, BadRequest for validation errors
                if (response.Errors == null || response.Errors.Count == 0)
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteSupplierCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}