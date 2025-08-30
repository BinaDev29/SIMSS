//using Application.CQRS.DeliveryDetails.Commands.CreateDeliveryDetail;
//using Application.CQRS.DeliveryDetails.Commands.DeleteDeliveryDetail;
//using Application.CQRS.DeliveryDetails.Commands.UpdateDeliveryDetail;
//using Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailById;
//using Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailList;
//using Application.DTOs.Delivery;
//using Application.Responses;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DeliveryDetailController(IMediator mediator) : ControllerBase
//    {
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult<IEnumerable<DeliveryDetailDto>>> GetAll(CancellationToken cancellationToken)
//        {
//            var result = await mediator.Send(new GetDeliveryDetailListQuery(), cancellationToken);
//            return Ok(result);
//        }

//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<DeliveryDetailDto>> Get(int id, CancellationToken cancellationToken)
//        {
//            var result = await mediator.Send(new GetDeliveryDetailByIdQuery { Id = id }, cancellationToken);

//            if (result is null)
//            {
//                return NotFound();
//            }

//            return Ok(result);
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult<BaseCommandResponse>> Create([FromBody] CreateDeliveryDetailDto dto, CancellationToken cancellationToken)
//        {
//            var command = new CreateDeliveryDetailCommand { DeliveryDetailDto = dto };
//            var response = await mediator.Send(command, cancellationToken);

//            if (!response.Success)
//            {
//                return BadRequest(response);
//            }

//            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
//        }

//        [HttpPut("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<BaseCommandResponse>> Update(int id, [FromBody] UpdateDeliveryDetailDto dto, CancellationToken cancellationToken)
//        {
//            var command = new UpdateDeliveryDetailCommand { Id = id, DeliveryDetailDto = dto };
//            var response = await mediator.Send(command, cancellationToken);

//            if (!response.Success)
//            {
//                return NotFound(response);
//            }

//            return Ok(response);
//        }

//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
//        {
//            var command = new DeleteDeliveryDetailCommand { Id = id };
//            var response = await mediator.Send(command, cancellationToken);

//            if (!response.Success)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }
//    }
//}