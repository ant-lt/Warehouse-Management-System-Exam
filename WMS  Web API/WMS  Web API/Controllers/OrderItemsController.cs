using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastructure.Interfaces;

namespace WMS__Web_API.Controllers
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

        public OrderItemsController(IOrderItemRepository orderItemRepo, ILogger<OrderItemsController> logger, IWMSwrapper wrapper)
        {
            _orderItemRepo = orderItemRepo ;
            _logger = logger ;
            _wrapper = wrapper ;
        }

        /// <summary>
        /// Create new Order item
        /// </summary>
        /// <param name="req"> New Order item data</param>
        /// <returns>Created new Order item</returns>
        /// <response code="201">Order created</response>
        /// <response code="500">Error</response>
        /// <response code="400">Bad request</response>
        [HttpPost("/CreateOrderItem", Name = "CreateNewOrderItem")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderItemDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateOrderItemDto>> Create(CreateOrderItemDto req)
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

                return CreatedAtRoute("CreateNewOrderItem", new { id = orderItem.Id }, req);


            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost CreateOrderItem exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete Order item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Order item deleted</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>  
        /// <response code="404">Order item not found</response>
        /// <response code="500">Internal server error</response>
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
                if (id == 0)
                {
                    return BadRequest();
                }

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
        /// Update Order item by id
        /// </summary>
        /// <param name="id"> Order item Id</param>
        /// <param name="updateOrderItemDto"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Order item updated</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Client could not authenticate a request</response>   
        /// <response code="403">Do not have permission to access</response> 
        /// <response code="404">Order item not found</response>
        /// <response code="500">Internal server error</response> 
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
                if (id == 0 || updateOrderItemDto == null)
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
        /// Fetch registered order single item by ID
        /// </summary>
        /// <param name="id">Requested order item ID</param>
        /// <returns>Order item for specified order item ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Order item bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="404">Order item not found </response>
        /// <response code="500">Internal server error</response>
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
                if (id == 0)
                {
                    return BadRequest();
                }

               
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
        /// Fetch registered order items related to specified order ID
        /// </summary>
        /// <param name="id">Requested order ID</param>
        /// <returns>Order items for specified order ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Order bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="404">Order not found </response>
        /// <response code="500">Internal server error</response>
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
                if (id == 0)
                {
                    return BadRequest();
                }

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
