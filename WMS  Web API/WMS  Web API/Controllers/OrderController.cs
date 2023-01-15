using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastructure.Interfaces;

namespace WMS__Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInventoryManagerService _inventoryManagerService;
        private readonly ILogger<OrderController> _logger;
        private readonly IWMSwrapper _wrapper;

        public OrderController(IOrderRepository orderRepo, ILogger<OrderController> logger, IWMSwrapper wrapper, IInventoryManagerService inventoryManagerService)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _wrapper = wrapper;
            _inventoryManagerService = inventoryManagerService;
        }

        /// <summary>
        /// Fetches all orders
        /// </summary>
        /// <returns>All orders in DB</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>  
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetOrders",Name = "GetOrders")]
        [Authorize(Roles = "Administrator, Manager, Supervisor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetOrderDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrders()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrders.");

            try
            {
                
                var orders = await _orderRepo.GetAllAsync( null, new List<string> { "OrderStatus", "OrderType", "Customer", "RWMSuser" });

                IEnumerable<GetOrderDto> getOrderDto = orders.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getOrderDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrders exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create new Order
        /// </summary>
        /// <param name="req"> New Order data</param>
        /// <returns>Created new Order</returns>
        /// <response code="201">Order created</response>
        /// <response code="500">Error</response>
        /// <response code="400">Bad request</response>
        [HttpPost("/CreateNewOrder", Name = "CreateNewOrder")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateOrderDto>> Create(CreateOrderDto req)
        {
            _logger.LogInformation($"{DateTime.Now} Executed Create new Order.");

            try
            {
                if (req == null)
                {
                    return BadRequest();
                }

                Order Order = _wrapper.Bind(req);
                await _orderRepo.CreateAsync(Order);

                return CreatedAtRoute("CreateNewOrder", new { id = Order.Id }, req);
        

            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost CreateOrder exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Order deleted</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response> 
        /// <response code="404">Order not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("/Delete/Order/{id:int}")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed DeleteOrder id = {id}.");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var Order = await _orderRepo.GetAsync(d => d.Id == id);

                if (Order == null)
                {
                    return NotFound();
                }

                await _orderRepo.RemoveAsync(Order);

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpDelete DeleteOrderById(id = {id}) exception error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Update Order by id
        /// </summary>
        /// <param name="id"> Order Id</param>
        /// <param name="updateOrderDto"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Order updated</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Client could not authenticate a request</response>   
        /// <response code="403">Do not have permission to access</response>  
        /// <response code="404">Order not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("/Update/Order/{id:int}")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> UpdateOrder(int id, UpdateOrderDto updateOrderDto)
        {
            _logger.LogInformation($"{DateTime.Now} Executed UpdateOrder id = {id}.");

            try
            {
                if (id == 0 || updateOrderDto == null)
                {
                    return BadRequest();
                }

                var foundOrder = await _orderRepo.GetAsync(d => d.Id == id);

                if (foundOrder == null)
                {
                    return NotFound();
                }

                await _orderRepo.UpdateAsync(_wrapper.Bind(updateOrderDto, foundOrder));

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPut UpdateOrderById(id = {id}) exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Fetch registered order with a specified ID from DB
        /// </summary>
        /// <param name="id">Requested order ID</param>
        /// <returns>Order with specified ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Order bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="404">Order not found </response>
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetOrderBy/{id:int}", Name = "GetOrderById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<GetOrderDto>> GetOrderById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrderById = {id}");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                
                var order = await _orderRepo.GetAsync(x => x.Id == id, new List<string> { "OrderStatus", "OrderType" });

                if (order == null)
                {
                    _logger.LogInformation($"{DateTime.Now} order with id {id} not found", id);
                    return NotFound();
                }

                return Ok(_wrapper.Bind(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrder by id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Submit the new order for processing
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>OK</returns>
        /// <response code="200">Order submitted</response>
        /// <response code="500">Error</response>
        /// <response code="400">Bad request</response>
        [HttpPost("/SubmitOrder/{id:int}", Name = "SubmitOrder")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmitOrderResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<SubmitOrderResponse>> SubmitOrder(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed Submit Order id={id}.");

            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var IsOrderTranfered = await _inventoryManagerService.ProcessOrderAsync(id);

                if (!IsOrderTranfered)
                {
                    return BadRequest();
                }

                var response = new SubmitOrderResponse() 
                { 
                    OrderDate= DateTime.Now,
                    OrderId= id,
                    OrderStatus = "Complete"
                };

                return Ok(response);
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost Submit Order id={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



    }
}
