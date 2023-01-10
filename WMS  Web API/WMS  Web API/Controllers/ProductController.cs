using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastrukture.Interfaces;

namespace WMS__Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<UserController> _logger;
        private readonly IWMSwrapper _wrapper;

        public ProductController(IProductRepository productRepo, ILogger<UserController> logger, IWMSwrapper wmsWrapper)
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
        [HttpGet(Name = "GetProducts")]
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
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetProducts nuluzo.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
