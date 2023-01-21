using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.Controllers
{
    /// <summary>
    /// Customers management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ILogger<CustomerController> _logger;
        private readonly IWMSwrapper _wrapper;

        public CustomerController(ICustomerRepository customerRepo, ILogger<CustomerController> logger, IWMSwrapper wmsWrapper)
        {
            _customerRepo = customerRepo ;
            _logger = logger;
            _wrapper = wmsWrapper;
        }

        /// <summary>
        /// Fetches all customers
        /// </summary>
        /// <returns>All customers in DB</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>        
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetCustomers", Name = "GetCustomers")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCustomerDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetCustomerDto>>> GetCustomers()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetCustomers.");

            try
            {
                var customers = await _customerRepo.GetAllAsync();


                IEnumerable<GetCustomerDto> getCustomerDto = customers.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getCustomerDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetCustomers exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="req"> New customer data</param>
        /// <returns>Created new Customer</returns>
        /// <response code="201">Customer created</response>
        /// <response code="500">Error</response>
        /// <response code="400">Bad request</response>
        /// <response code="403">Do not have permission to access</response>
        [HttpPost("/CreateNewCustomer", Name = "CreateNewCustomer")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCustomerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateCustomerDto>> Create(CreateCustomerDto req)
        {
            _logger.LogInformation($"{DateTime.Now} Executed Create new Customer.");

            try
            {
                if (req == null)
                {
                    return BadRequest();
                }

                Customer customer = _wrapper.Bind(req);
                await _customerRepo.CreateAsync(customer);
               
                return CreatedAtRoute("CreateNewCustomer", new { id = customer.Id }, req);

            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPost CreateCustomer exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Delete Customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Customer deleted</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>      
        /// <response code="404">Customer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("/Delete/Customer/{id:int}")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed DeleteCustomer id = {id}.");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var Customer = await _customerRepo.GetAsync(d => d.Id == id);

                if (Customer == null)
                {
                    return NotFound();
                }

                await _customerRepo.RemoveAsync(Customer);

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpDelete DeleteCustomerById(id = {id}) exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Update Customer by id
        /// </summary>
        /// <param name="id"> Customer Id</param>
        /// <param name="updateCustomerDto"></param>
        /// <returns>Status code</returns>
        /// <response code="204">Customer updated</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>
        /// <response code="404">Customer not found</response>
        /// <response code="500">Internal server error</response> 
        [HttpPut("/Update/Customer/{id:int}")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
        {
            _logger.LogInformation($"{DateTime.Now} Executed UpdateCustomer id = {id}.");

            try
            {
                if (id == 0 || updateCustomerDto == null)
                {
                    return BadRequest();
                }

                var foundCustomer = await _customerRepo.GetAsync(d => d.Id == id);

                if (foundCustomer == null)
                {
                    return NotFound();
                }
               
                await _customerRepo.UpdateAsync(_wrapper.Bind(updateCustomerDto, foundCustomer) );

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPut UpdateCustomerById(id = {id}) exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Fetch registered customer with a specified ID from DB
        /// </summary>
        /// <param name="id">Requested customer ID</param>
        /// <returns>Customer with specified ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Customer bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>
        /// <response code="404">Customer not found </response>
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetCustomerBy/{id:int}", Name = "GetCustomerById")]
        [Authorize(Roles = "Administrator, Manager")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<GetCustomerDto>> GetCustomerById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetCustomerById = {id}");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var customer = await _customerRepo.GetAsync(d => d.Id == id);

                if (customer == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Customer with id {id} not found", id);
                    return NotFound();
                }

                return Ok(_wrapper.Bind(customer));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetCustomer by id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
