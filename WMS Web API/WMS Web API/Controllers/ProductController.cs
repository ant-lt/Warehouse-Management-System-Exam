using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.Controllers
{
    /// <summary>
    /// Products management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<CustomerController> _logger;
        private readonly IWMSwrapper _wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productRepo">An instance of the product repository interface responsible for data access.</param>
        /// <param name="logger">An instance of the logger interface for logging controller actions and events.</param>
        /// <param name="wmsWrapper">An instance of the Warehouse Management System (WMS) wrapper interface for integration.</param>
        public ProductController(IProductRepository productRepo, ILogger<CustomerController> logger, IWMSwrapper wmsWrapper)
        {
            _productRepo = productRepo ;
            _logger = logger;
            _wrapper = wmsWrapper;
        }

        /// <summary>
        /// Retrieves a list of all products from the database.
        /// </summary>
        /// <returns>A collection containing all products in the database.</returns>
        /// <response code="200">OK: Returns a list of all products.</response>
        /// <response code="401">Unauthorized: The client does not have the necessary authentication credentials.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetProducts", Name = "GetProducts")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetProducts()        
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetProducts.");

            try
            {
                var products = await _productRepo.GetAllAsync();

                 IEnumerable<GetProductDto> getProductDto = products.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getProductDto);
             
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetProducts exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Retrieves detailed information of a registered product with a specified ID from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the requested product.</param>
        /// <returns>The product data with the specified ID.</returns>
        /// <response code="200">OK: The product information was successfully retrieved.</response>        
        /// <response code="400">Bad Request: The requested product is invalid.</response>
        /// <response code="401">Unauthorized Access: You do not have the necessary permission to access the requested product.</response>
        /// <response code="404">Not Found: The requested product could not be found.</response>
        /// <response code="500">Internal Server Error: The server encountered an unexpected condition that prevented it from fulfilling the request.</response>
        [HttpGet("/GetProductBy/{id:int}", Name = "GetProductById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<GetProductDto>> GetProductById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetProductById = {id}");

            try
            {
                var product = await _productRepo.GetAsync(d => d.Id == id);

                if (product == null)
                {
                    _logger.LogInformation($"{DateTime.Now} product with id {id} not found", id);
                    return NotFound();
                }

                return Ok(_wrapper.Bind(product));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetProduct by id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

    }
}