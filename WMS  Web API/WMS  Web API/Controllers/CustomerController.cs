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
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetCustomers")]
   //     [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCustomerDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        
        [HttpPost("Create", Name = "CreateNewCustomer")]
 //       [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCustomerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="404">Customer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{id:int}")]
    //    [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="404">Customer not found</response>
        /// <response code="500">Internal server error</response> 
        /// <remarks>
        /// Sample request:
        ///
        ///         PUT Customers/1
        ///         {
        ///             "id": 0,
        ///             "isleista": "2022-12-04T11:36:24.011Z",
        ///             "autorius": "string",
        ///             "pavadinimas": "string",
        ///             "knygosTipas": "string"  - allowed only from values list: Hardcover, Paperback, Electronic
        ///         }
        ///
        /// </remarks>
        [HttpPut("update/{id:int}")]
  //      [Authorize(Roles = "Administrator,Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <response code="404">Customer not found </response>
        /// <response code="500">Internal server error</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET customer/1
        ///     {
        ///     }
        ///
        /// </remarks>
        [HttpGet("{id:int}", Name = "GeCustomerById")]
  //      [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
