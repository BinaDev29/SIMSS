//// API/Controllers/InvoiceController.cs
//using Application.CQRS.Invoices.Commands.CreateInvoice;
//using Application.CQRS.Invoices.Commands.DeleteInvoice;
//using Application.CQRS.Invoices.Queries.GetInvoiceById;
//using Application.CQRS.Invoices.Queries.GetInvoiceList;
//using Application.DTOs.Invoice;
//using Application.Responses;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class InvoiceController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public InvoiceController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        /// <summary>
//        /// Get all invoices
//        /// </summary>
//        /// <returns>List of invoices</returns>
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult> GetInvoices()
//        {
//            try
//            {
//                // ✅ የተስተካከለው ኮድ: የ'required' ስህተትን ያስተካክላል
//                var query = new GetInvoiceListQuery
//                {
//                    Parameters = new PagedListParameters()
//                };
//                var result = await _mediator.Send(query);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while retrieving invoices.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Get invoice by ID
//        /// </summary>
//        /// <param name="id">Invoice ID</param>
//        /// <returns>Invoice details</returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
//        {
//            try
//            {
//                var query = new GetInvoiceByIdQuery { Id = id };
//                var result = await _mediator.Send(query);

//                if (result == null)
//                {
//                    return NotFound(new { message = $"Invoice with ID {id} not found." });
//                }

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while retrieving the invoice.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Create a new invoice
//        /// </summary>
//        /// <param name="createInvoiceDto">Invoice creation data</param>
//        /// <returns>Creation result</returns>
//        [HttpPost]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var command = new CreateInvoiceCommand { InvoiceDto = createInvoiceDto };
//                var result = await _mediator.Send(command);

//                if (!result.Success)
//                {
//                    return BadRequest(result);
//                }

//                return CreatedAtAction(nameof(GetInvoice), new { id = result.Id }, result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while creating the invoice.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Delete an invoice
//        /// </summary>
//        /// <param name="id">Invoice ID</param>
//        /// <returns>Deletion result</returns>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> DeleteInvoice(int id)
//        {
//            try
//            {
//                var command = new DeleteInvoiceCommand { Id = id };
//                var result = await _mediator.Send(command);

//                // ✅ የተስተካከለው ኮድ: የ'not found' ስህተትን ያስተካክላል
//                if (!result.Success)
//                {
//                    if (result.Id == 0) // መታወቂያው 0 ከሆነ የለም ማለት ነው ብለን እንገምታለን
//                    {
//                        return NotFound(result);
//                    }
//                    return BadRequest(result);
//                }

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while deleting the invoice.", error = ex.Message });
//            }
//        }
//    }
//}