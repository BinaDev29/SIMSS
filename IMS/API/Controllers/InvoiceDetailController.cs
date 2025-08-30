//using Application.CQRS.InvoiceDetails.Commands.CreateInvoiceDetail;
//using Application.CQRS.InvoiceDetails.Commands.DeleteInvoiceDetail;
//using Application.CQRS.InvoiceDetails.Commands.UpdateInvoiceDetail;
//using Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailById;
//using Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailList;
//using Application.DTOs.Invoice;
//using Application.Responses;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class InvoiceDetailController(IMediator mediator) : ControllerBase
//    {
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult<IEnumerable<InvoiceDetailDto>>> GetAll(CancellationToken cancellationToken)
//        {
//            var result = await mediator.Send(new GetInvoiceDetailListQuery(), cancellationToken);
//            return Ok(result);
//        }

//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<InvoiceDetailDto>> Get(int id, CancellationToken cancellationToken)
//        {
//            var result = await mediator.Send(new GetInvoiceDetailByIdQuery { Id = id }, cancellationToken);

//            if (result is null)
//            {
//                return NotFound();
//            }

//            return Ok(result);
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult> Create([FromBody] CreateInvoiceDetailDto dto, CancellationToken cancellationToken)
//        {
//            var command = new CreateInvoiceDetailCommand { InvoiceDetailDto = dto };
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
//        public async Task<ActionResult> Update(int id, [FromBody] UpdateInvoiceDetailDto dto, CancellationToken cancellationToken)
//        {
//            var command = new UpdateInvoiceDetailCommand { Id = id, InvoiceDetailDto = dto };
//            var response = await mediator.Send(command, cancellationToken);

//            if (!response.Success)
//            {
//                if (response.Errors is not null && response.Errors.Contains("Invoice Detail not found."))
//                {
//                    return NotFound(response);
//                }
//                return BadRequest(response);
//            }

//            return Ok(response);
//        }

//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
//        {
//            var command = new DeleteInvoiceDetailCommand { Id = id };
//            var response = await mediator.Send(command, cancellationToken);

//            if (!response.Success)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }
//    }
//}