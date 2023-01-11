using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models.DTO;
using WMS.Infastructure.Interfaces;

namespace WMS__Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepo;
        private readonly ILogger<InventoryController> _logger;
        private readonly IWMSwrapper _wrapper;

        public InventoryController(IInventoryRepository inventoryRepo, ILogger<InventoryController> logger, IWMSwrapper wrapper)
        {
            _inventoryRepo = inventoryRepo;
            _logger = logger;
            _wrapper = wrapper;
        }

        /// <summary>
        /// Fetches all inventories
        /// </summary>
        /// <returns>All inventories in DB</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetInventories")]
        //     [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInventoryDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetInventoryDto>>> GetInventories()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetInventories.");

            try
            {
                var inventories = await _inventoryRepo.GetAllAsync();


                IEnumerable<GetInventoryDto> getInventoryDto = inventories.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getInventoryDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetInventories nuluzo.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
