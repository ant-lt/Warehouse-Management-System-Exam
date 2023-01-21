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

        public ProductController(IProductRepository productRepo, ILogger<CustomerController> logger, IWMSwrapper wmsWrapper)
        {
            _productRepo = productRepo ;
            _logger = logger;
            _wrapper = wmsWrapper;
        }


        /// <summary>
        /// Fetches all products
        /// </summary>
        /// <returns>All product in DB</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="500">Internal server error</response>
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
        /// Fetch registered product details with a specified ID from DB
        /// </summary>
        /// <param name="id">Requested product ID</param>
        /// <returns>Product with specified ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Product bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="404">Product not found </response>
        /// <response code="500">Internal server error</response>
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
                if (id == 0)
                {
                    return BadRequest();
                }

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
