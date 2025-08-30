using Application.CQRS.User.Commands.CreateUser;
using Application.CQRS.User.Commands.DeleteUser;
using Application.CQRS.User.Queries.GetUserById;
using Application.CQRS.User.Queries.GetUserList;
using Application.CQRS.User.Commands.UpdateUser;

using Application.DTOs.User;
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
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserListQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand { UserDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                // Return Bad Request for validation errors
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand { Id = id, UserDto = dto };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                // Differentiate between Not Found and Bad Request
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
            var command = new DeleteUserCommand { Id = id };
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}