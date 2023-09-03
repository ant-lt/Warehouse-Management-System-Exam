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

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController" /> class.
        /// </summary>
        /// <param name="customerRepo">An instance of the customer repository interface responsible for data access.</param>
        /// <param name="logger">An instance of the logger interface for logging controller actions and events.</param>
        /// <param name="wmsWrapper">An instance of the Warehouse Management System (WMS) wrapper interface for integration.</param>
        public CustomerController(ICustomerRepository customerRepo, ILogger<CustomerController> logger, IWMSwrapper wmsWrapper)
        {
            _customerRepo = customerRepo;
            _logger = logger;
            _wrapper = wmsWrapper;
        }


        /// <summary>
        /// Retrieves a list of all customers from the database.
        /// </summary>
        /// <returns>An array containing all customers in the database.</returns>
        /// <response code="200">The request was successful, and the list of customers is returned.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response>        
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
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
        /// Creates a new customer with the provided customer details.
        /// </summary>
        /// <param name="req">The customer details to be used for creation.</param>
        /// <returns>The newly created customer's response.</returns>
        /// <response code="201">Created: The customer was successfully created.</response>
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response> 
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
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
        /// Deletes a customer with the specified customer Id from the database.
        /// </summary>
        /// <param name="id">The Id of the customer to be deleted.</param>
        /// <response code="204">No Content: The customer was successfully deleted.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response>        
        /// <response code="404">Not Found: The specified customer was not found in the database.</response>
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
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
        /// Updates a customer with the provided Id using the supplied customer details.
        /// </summary>
        /// <param name="id">The Id of the customer to be updated.</param>
        /// <param name="updateCustomerDto">The customer details used for updating.</param>
        /// <returns>204 No Content: The customer was successfully updated.</returns>
        /// <response code="204">No Content: The customer was successfully updated.</response>
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response> 
        /// <response code="404">Not Found: The specified customer was not found in the database.</response>
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
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
                var customer = await _customerRepo.GetAsync(d => d.Id == id);

                if (customer == null)
                {
                    return NotFound();
                }

                _wrapper.Bind(updateCustomerDto, customer);
                await _customerRepo.UpdateAsync(customer);

                return NoContent();
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpPut UpdateCustomer exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Retrieves a customer with the specified customer Id.
        /// </summary>
        /// <param name="id">The Id of the customer to retrieve.</param>
        /// <returns>The customer with the given Id.</returns>
        /// <response code="200">OK: The customer was found and retrieved successfully.</response>
        /// <response code="400">Bad Request: The request is invalid or missing required data.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="403">Forbidden: The client is not allowed to access this resource.</response> 
        /// <response code="404">Not Found: The specified customer was not found in the database.</response>
        /// <response code="500">Internal Server Error: An error occurred while processing the request on the server.</response>
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
            _logger.LogInformation($"{DateTime.Now} Executed GetCustomerById id = {id}.");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var customer = await _customerRepo.GetAsync(d => d.Id == id);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(_wrapper.Bind(customer) as GetCustomerDto);
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetCustomerById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
