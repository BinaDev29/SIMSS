//using Application.CQRS.Suppliers.Commands.CreateSupplier;
//using Application.CQRS.Suppliers.Commands.DeleteSupplier;
//using Application.CQRS.Suppliers.Commands.UpdateSupplier;
//using Application.CQRS.Suppliers.Queries.GetSupplierById;
//using Application.CQRS.Suppliers.Queries.GetSupplierList;
//using Application.DTOs.Supplier;
//using Application.Responses;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class SupplierController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public SupplierController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        /// <summary>
//        /// Get all suppliers
//        /// </summary>
//        /// <returns>List of suppliers</returns>
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult> GetSuppliers()
//        {
//            try
//            {
//                var query = new GetSupplierListQuery();
//                var result = await _mediator.Send(query);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, 
//                    new { message = "An error occurred while retrieving suppliers.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Get supplier by ID
//        /// </summary>
//        /// <param name="id">Supplier ID</param>
//        /// <returns>Supplier details</returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
//        {
//            try
//            {
//                var query = new GetSupplierByIdQuery { Id = id };
//                var result = await _mediator.Send(query);
                
//                if (result == null)
//                {
//                    return NotFound(new { message = $"Supplier with ID {id} not found." });
//                }
                
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, 
//                    new { message = "An error occurred while retrieving the supplier.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Create a new supplier
//        /// </summary>
//        /// <param name="createSupplierDto">Supplier creation data</param>
//        /// <returns>Creation result</returns>
//        [HttpPost]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> CreateSupplier([FromBody] CreateSupplierDto createSupplierDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var command = new CreateSupplierCommand { SupplierDto = createSupplierDto };
//                var result = await _mediator.Send(command);
                
//                if (!result.Success)
//                {
//                    return BadRequest(result);
//                }
                
//                return CreatedAtAction(nameof(GetSupplier), new { id = result.Id }, result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, 
//                    new { message = "An error occurred while creating the supplier.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Update an existing supplier
//        /// </summary>
//        /// <param name="id">Supplier ID</param>
//        /// <param name="updateSupplierDto">Supplier update data</param>
//        /// <returns>Update result</returns>
//        [HttpPut("{id}")]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> UpdateSupplier(int id, [FromBody] UpdateSupplierDto updateSupplierDto)
//        {
//            try
//            {
//                if (id != updateSupplierDto.Id)
//                {
//                    return BadRequest(new { message = "ID mismatch between route and body." });
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var command = new UpdateSupplierCommand { SupplierDto = updateSupplierDto };
//                var result = await _mediator.Send(command);
                
//                if (!result.Success)
//                {
//                    if (result.Errors.Any(e => e.Contains("not found")))
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
//                    new { message = "An error occurred while updating the supplier.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Delete a supplier
//        /// </summary>
//        /// <param name="id">Supplier ID</param>
//        /// <returns>Deletion result</returns>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> DeleteSupplier(int id)
//        {
//            try
//            {
//                var command = new DeleteSupplierCommand { Id = id };
//                var result = await _mediator.Send(command);
                
//                if (!result.Success)
//                {
//                    if (result.Errors.Any(e => e.Contains("not found")))
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
//                    new { message = "An error occurred while deleting the supplier.", error = ex.Message });
//            }
//        }
//    }
//}