using Application.CQRS.Employees.Commands.CreateEmployee;
using Application.CQRS.Employees.Commands.DeleteEmployee;
using Application.CQRS.Employees.Commands.UpdateEmployee;
using Application.CQRS.Employees.Queries.GetEmployeeById;
using Application.CQRS.Employees.Queries.GetEmployeeList;
using Application.DTOs.Employee;
using Application.DTOs.Common;
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
    public class EmployeeController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees([FromQuery] EmployeeQueryParameters parameters, CancellationToken cancellationToken)
        {
            var query = new GetEmployeeListQuery { Parameters = parameters };
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetEmployeeByIdQuery { Id = id }, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> CreateEmployee([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateEmployeeCommand { EmployeeDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetEmployee), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateEmployeeCommand { Id = id, EmployeeDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (response.Success == false)
            {
                if (response.Errors != null && response.Errors.Contains("Employee not found."))
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
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}