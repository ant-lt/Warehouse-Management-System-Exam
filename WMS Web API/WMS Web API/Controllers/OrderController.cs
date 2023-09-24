using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Xml.Serialization;
using WMS.Domain.Models;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.Controllers
{
    /// <summary>
    /// Orders management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInventoryManagerService _inventoryManagerService;
        private readonly ILogger<OrderController> _logger;
        private readonly IWMSwrapper _wrapper;

        /// <summary>
        /// Constructor for OrderController
        /// </summary>
        /// <param name="orderRepo">IOrderRepository object</param>
        /// <param name="logger">ILogger object</param>
        /// <param name="wrapper">IWMSwrapper object</param>
        /// <param name="inventoryManagerService">IInventoryManagerService object</param>
        public OrderController(IOrderRepository orderRepo, ILogger<OrderController> logger, IWMSwrapper wrapper, IInventoryManagerService inventoryManagerService)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _wrapper = wrapper;
            _inventoryManagerService = inventoryManagerService;
        }

        /// <summary>
        /// Retrieves a list of all orders from the database and logs the operation.
        /// </summary>
        /// <returns>A collection containing all orders from the database.</returns>
        /// <response code="200">OK: Returns the list of all orders.</response>
        /// <response code="401">Unauthorized: The caller is not authenticated.</response>
        /// <response code="403">Forbidden: The caller is authenticated but not authorized to access orders.</response>  
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetOrders",Name = "GetOrders")]
        [Authorize]
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
        /// Creates a new order using the provided order data.
        /// </summary>
        /// <param name="req">The data for the new order.</param>
        /// <returns>The created order.</returns>
        /// <response code="201">Created: Returns the newly created order.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpPost("/CreateNewOrder", Name = "CreateNewOrder")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateNewResourceResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateNewResourceResponseDto>> Create(CreateOrderDto req)
        {
            _logger.LogInformation($"{DateTime.Now} Executed Create new Order.");

            try
            {
                if (req == null)
                {
                    return BadRequest();
                }

                Order order = _wrapper.Bind(req);
                await _orderRepo.CreateAsync(order);

                CreateNewResourceResponseDto createOrderResponseDto = new CreateNewResourceResponseDto()
                { 
                    Id = order.Id
                };
                return CreatedAtRoute(nameof(GetOrderById), new { id = order.Id }, createOrderResponseDto);
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost CreateOrder exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the specified order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to be deleted.</param>
        /// <returns>No Content upon successful deletion.</returns>
        /// <response code="204">No Content: The order was successfully deleted.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The caller is not authenticated.</response>
        /// <response code="403">Forbidden: The caller is authenticated but not authorized to delete orders.</response> 
        /// <response code="404">Not Found: The specified order to be deleted was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpDelete("/Delete/Order/{id:int}")]
        [Authorize(Roles = "Administrator, Supervisor")]
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
        /// Updates the specified order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to be updated.</param>
        /// <param name="updateOrderDto">The data used for updating the order.</param>
        /// <returns>The HTTP status code indicating the result of the update operation.</returns>
        /// <response code="204">No Content: The order was successfully updated.</response>
        /// <response code="400">Bad Request: Indicates an invalid request or missing required data.</response>
        /// <response code="401">Unauthorized: The caller is not authenticated.</response>   
        /// <response code="403">Forbidden: The caller is authenticated but not authorized to update orders.</response>  
        /// <response code="404">Not Found: The specified order to be updated was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpPut("/Update/Order/{id:int}")]
        [Authorize(Roles = "Customer")]
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
                if (updateOrderDto == null)
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
        /// Retrieves a specific order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to retrieve.</param>
        /// <returns>The retrieved order.</returns>
        /// <response code="200">OK: Returns the order.</response>
        /// <response code="400">Bad Request: The order ID is not valid or the request is invalid.</response>
        /// <response code="401">Unauthorized: The client is not authorized to access the resource.</response>
        /// <response code="404">Not Found: The order with the specified ID was not found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
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
                var order = await _orderRepo.GetAsync(x => x.Id == id, new List<string> { "OrderStatus", "OrderType", "Customer", "RWMSuser" });

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
        /// Submits a new order for processing with the specified Order Id.
        /// </summary>
        /// <param name="id">The unique identifier of the order to be submitted.</param>
        /// <returns>OK</returns>
        /// <response code="200">OK: The order was successfully submitted for processing.</response>
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="500">Internal Server Error: An error occurred while processing the order.</response>
        [HttpPost("/SubmitOrder/{id:int}", Name = "SubmitOrder")]
        [Authorize(Roles = "Customer")]
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


        /// <summary>
        /// Retrieves a list of all order statuses.
        /// </summary>
        /// <returns>A list of order statuses.</returns>
        /// <response code="200">OK: Returns a list of all order statuses.</response>        
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="401">Unauthorized: The client could not authenticate the request.</response>
        /// <response code="404">Not Found: No order statuses were found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetOrderStatuses", Name = "GetOrderStatuses")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetOrderStatusDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetOrderStatusDto>>> GetOrderStatuses()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrderStatuses ");

            try
            {
                var orderStatuses = await _orderRepo.GetOrderStatusListAsync();

                IEnumerable<GetOrderStatusDto> getOrderStatusDto = orderStatuses.Select(d => _wrapper.Bind(d)).ToList();
                return Ok(getOrderStatusDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrderStatuse exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Retrieves a list of all order types.
        /// </summary>
        /// <returns>A list of order types.</returns>
        /// <response code="200">OK: Returns a list of all order types.</response>        
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="401">Unauthorized: The client could not authenticate the request.</response>
        /// <response code="404">Not Found: No order types were found.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetOrderTypes", Name = "GetOrderTypes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetOrderTypesDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetOrderTypesDto>>> GetOrderTypes()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetOrderTypes");

            try
            {
                var orderTypes = await _orderRepo.GetOrderTypesListAsync();

                IEnumerable<GetOrderTypesDto> getOrderTypesDto = orderTypes.Select(d => _wrapper.Bind(d)).ToList();
                return Ok(getOrderTypesDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetOrderTypes exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
