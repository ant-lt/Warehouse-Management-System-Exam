using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;

namespace WMS_Web_API.Controllers
{

    /// <summary>
    /// Inventories reporting
    /// </summary>
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
        /// <response code="403">Do not have permission to access</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetInventories", Name = "GetInventories")]
        [Authorize(Roles = "Administrator, Manager, Supervisor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInventoryDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetInventoryDto>>> GetInventories()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetInventories.");

            try
            {                
                var inventories = await _inventoryRepo.GetAllAsync(null, new List<string> { "Warehouse", "Product" });

                IEnumerable<GetInventoryDto> getInventoryDto = inventories.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getInventoryDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetInventories exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all warehouses ratio of occupied
        /// </summary>
        /// <returns>Warehouse list with ratio of occupied</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="403">Do not have permission to access</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("/GetWarehousesRatioOfOccupied", Name = "GetWarehousesRatioOfOccupied")]
        [Authorize(Roles = "Administrator, Manager, Supervisor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetWarehousesRatioOfOccupiedDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetWarehousesRatioOfOccupiedDto>>> GetWarehousesRatioOfOccupied()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetWarehousesRatioOfOccupied.");

            try
            {
                var warehouses = await _inventoryRepo.GetWarehouseListAsync();

                List<GetWarehousesRatioOfOccupiedDto> getWarehousesRatioOfOccupiedDto = new List<GetWarehousesRatioOfOccupiedDto>();
                
                if (warehouses != null)
                { 
                    foreach (Warehouse warehouse in warehouses)
                    {
                        var warehouseRatioOfOccupied = await _inventoryRepo.GetWarehouseRatioOfOccupiedbyIdAsync(warehouse.Id);
                        var newGetWarehousesRatioOfOccupiedDto= new GetWarehousesRatioOfOccupiedDto();
                        newGetWarehousesRatioOfOccupiedDto = _wrapper.Bind(warehouse, warehouseRatioOfOccupied);
                        getWarehousesRatioOfOccupiedDto.Add(newGetWarehousesRatioOfOccupiedDto);
                    }
                }
                
                return Ok(getWarehousesRatioOfOccupiedDto.ToList());

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetWarehousesRatioOfOccupied exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
