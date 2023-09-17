using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;

namespace WMS_Web_API.Controllers
{
    /// <summary>
    /// Orders items management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IWMSwrapper _wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemsController" /> class.
        /// </summary>
        /// <param name="orderItemRepo">An instance of the order item repository interface responsible for data access.</param>
        /// <param name="logger">An instance of the logger interface for logging controller actions and events.</param>
        /// <param name="wrapper">An instance of the Warehouse Management System (WMS) wrapper interface for integration.</param>
        public OrderItemsController(IOrderItemRepository orderItemRepo, ILogger<OrderItemsController> logger, IWMSwrapper wrapper)
        {
            _orderItemRepo = orderItemRepo ;
            _logger = logger ;
            _wrapper = wrapper ;
        }

        /// <summary>
        /// Creates a new Order item using the provided order item data.
        /// </summary>
        /// <param name="req">The data for the new Order item.</param>
        /// <returns>The newly created Order item.</returns>
        /// <response code="201">Created: The Order item was successfully created.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response>        
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
        [HttpPost("/CreateOrderItem", Name = "CreateNewOrderItem")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateNewResourceResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateNewResourceResponseDto>> Create(CreateOrderItemDto req)
        {
            _logger.LogInformation($"{DateTime.Now} Executed Create new Order item.");

            try
            {
                if (req == null)
                {
                    return BadRequest();
                }

                OrderItem orderItem = _wrapper.Bind(req);
                await _orderItemRepo.CreateAsync(orderItem);
               
                CreateNewResourceResponseDto createOrderItemResponse = new CreateNewResourceResponseDto()
                {
                    Id = orderItem.Id
                };

                return CreatedAtRoute(nameof(GetOrderItemById), new { id = orderItem.Id }, createOrderItemResponse);
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost CreateOrderItem exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes an Order item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Order item to be deleted.</param>
        /// <returns>The HTTP status code indicating the result of the deletion.</returns>
        /// <response code="204">No Content: The Order item was successfully deleted.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client does not have permission to access this resource.</response>  
        /// <response code="404">Not Found: The specified Order item to be deleted was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpDelete("/Delete/OrderItem/{id:int}")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> DeleteOrderItem(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed DeleteOrderItem id = {id}.");

            try
            {
                var orderItem = await _orderItemRepo.GetAsync(d => d.Id == id);

                if (orderItem == null)
                {
                    return NotFound();
                }

                await _orderItemRepo.RemoveAsync(orderItem);

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpDelete DeleteOrderItemById(id = {id}) exception error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Updates an Order item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Order item to be updated.</param>
        /// <param name="updateOrderItemDto">The data used for updating the Order item.</param>
        /// <returns>The HTTP status code indicating the result of the update operation.</returns>
        /// <response code="204">No Content: The Order item was successfully updated.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>   
        /// <response code="403">Forbidden: The client does not have permission to access this resource.</response> 
        /// <response code="404">Not Found: The specified Order item to be updated was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpPut("/Update/OrderItem/{id:int}")]
        [Authorize(Roles = "Administrator,Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> UpdateOrderItem(int id, UpdateOrderItemDto updateOrderItemDto)
        {
            _logger.LogInformation($"{DateTime.Now} Executed UpdateOrderItem id = {id}.");

            try
            {
                if (updateOrderItemDto == null)
                {
                    return BadRequest();
                }

                var foundOrder = await _orderItemRepo.GetAsync(d => d.Id == id);

                if (foundOrder == null)
                {
                    return NotFound();
                }

                await _orderItemRepo.UpdateAsync(_wrapper.Bind(updateOrderItemDto, foundOrder));

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPut UpdateOrderItemById(id = {id}) exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a registered order item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the requested order item.</param>
        /// <returns>The order item corresponding to the specified order item ID.</returns>
        /// <response code="200">OK: Returns the requested order item.</response>        
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="404">Not Found: The specified order item was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetOrderItemBy/{id:int}", Name = "GetOrderItemById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderItemDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<GetOrderItemDto>> GetOrderItemById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrderItemById = {id}");

            try
            {
                var orderItems = await _orderItemRepo.GetAsync(x => x.Id == id, new List<string> { "Product" });

                if (orderItems == null)
                {
                    _logger.LogInformation($"{DateTime.Now} items for order with id {id} not found", id);
                    return NotFound();
                }

                return Ok(_wrapper.Bind(orderItems));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrderItem by order id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Retrieves registered order items related to the specified order ID.
        /// </summary>
        /// <param name="id">The unique identifier of the requested order.</param>
        /// <returns>The order items associated with the specified order ID.</returns>
        /// <response code="200">OK: Returns the registered order items.</response>        
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="404">Not Found: The specified order was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetOrderBy/{id:int}/Items", Name = "GetOrderItemsById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetOrderItemDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetOrderItemDto>>> GetOrderItemsById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrderItemsById = {id}");

            try
            {
                var order = await _orderItemRepo.GetAsync(d => d.OrderId == id);

                if (order == null)
                {
                    _logger.LogInformation($"{DateTime.Now} order with id {id} not found", id);
                    return NotFound();
                }

                var orderItems = await _orderItemRepo.GetAllAsync(x => x.OrderId == id, new List<string> { "Product" });

                IEnumerable<GetOrderItemDto> getOrderItemsDto = orderItems.Select(d => _wrapper.Bind(d));

                return Ok(getOrderItemsDto);
            
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrderItems by id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
