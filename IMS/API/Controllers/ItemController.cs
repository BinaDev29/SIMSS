//// API/Controllers/ItemController.cs
//using Application.CQRS.Items.Commands.CreateItem;
//using Application.CQRS.Items.Commands.DeleteItem;
//using Application.CQRS.Items.Commands.UpdateItem;
//using Application.CQRS.Items.Queries.GetItemById;
//using Application.CQRS.Items.Queries.GetItemList;
//using Application.DTOs.Item;
//using Application.Responses;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class ItemController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public ItemController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        /// <summary>
//        /// Get all items
//        /// </summary>
//        /// <returns>List of items</returns>
//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult> GetItems()
//        {
//            try
//            {
//                // ??? GetItemListQuery class ?? Parameters ???? required property ???
//                // var query = new GetItemListQuery { Parameters = new PagedListParameters() }; ??? ?????? ????
//                var query = new GetItemListQuery();
//                var result = await _mediator.Send(query);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while retrieving items.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Get item by ID
//        /// </summary>
//        /// <param name="id">Item ID</param>
//        /// <returns>Item details</returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ItemDto>> GetItem(int id)
//        {
//            try
//            {
//                var query = new GetItemByIdQuery { Id = id };
//                var result = await _mediator.Send(query);

//                if (result == null)
//                {
//                    return NotFound(new { message = $"Item with ID {id} not found." });
//                }

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while retrieving the item.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Create a new item
//        /// </summary>
//        /// <param name="createItemDto">Item creation data</param>
//        /// <returns>Creation result</returns>
//        [HttpPost]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> CreateItem([FromBody] CreateItemDto createItemDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var command = new CreateItemCommand { ItemDto = createItemDto };
//                var result = await _mediator.Send(command);

//                if (!result.Success)
//                {
//                    return BadRequest(result);
//                }

//                return CreatedAtAction(nameof(GetItem), new { id = result.Id }, result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new { message = "An error occurred while creating the item.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Update an existing item
//        /// </summary>
//        /// <param name="id">Item ID</param>
//        /// <param name="updateItemDto">Item update data</param>
//        /// <returns>Update result</returns>
//        [HttpPut("{id}")]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> UpdateItem(int id, [FromBody] UpdateItemDto updateItemDto)
//        {
//            try
//            {
//                if (id != updateItemDto.Id)
//                {
//                    return BadRequest(new { message = "ID mismatch between route and body." });
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var command = new UpdateItemCommand { ItemDto = updateItemDto };
//                var result = await _mediator.Send(command);

//                // ? ???????? ??
//                if (!result.Success)
//                {
//                    // ?????? 0 ??? ??? ??? ?? ??? ???????
//                    if (result.Id == 0)
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
//                    new { message = "An error occurred while updating the item.", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Delete an item
//        /// </summary>
//        /// <param name="id">Item ID</param>
//        /// <returns>Deletion result</returns>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<BaseCommandResponse>> DeleteItem(int id)
//        {
//            try
//            {
//                var command = new DeleteItemCommand { Id = id };
//                var result = await _mediator.Send(command);

//                // ? ???????? ??
//                if (!result.Success)
//                {
//                    // ?????? 0 ??? ??? ??? ?? ??? ???????
//                    if (result.Id == 0)
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
//                    new { message = "An error occurred while deleting the item.", error = ex.Message });
//            }
//        }
//    }
//}