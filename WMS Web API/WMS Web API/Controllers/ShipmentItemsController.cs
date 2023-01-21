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
    /// Shipments items reporting
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentItemsController : ControllerBase
    {
        private readonly IShipmentItemRepository _shipmentItemRepo;
        private readonly ILogger<ShipmentItemsController> _logger;
        private readonly IWMSwrapper _wrapper;

        public ShipmentItemsController(IShipmentItemRepository shipmentItemRepo, ILogger<ShipmentItemsController> logger, IWMSwrapper wrapper)
        {
            _shipmentItemRepo = shipmentItemRepo ;
            _logger = logger ;
            _wrapper = wrapper ;
        }

        /// <summary>
        /// Fetch registered shipment items with a specified shipment ID
        /// </summary>
        /// <param name="id">Requested shipment ID</param>
        /// <returns>Shipment items with specified ID</returns>
        /// <response code="200">OK</response>        
        /// <response code="400">Shipments bad request description</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="404">Shipment not found </response>
        /// <response code="500">Internal server error</response>
        [HttpGet("/ShipmentId/{id:int}/Items", Name = "GetShipmentItemsById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetShipmentItemDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetShipmentItemDto>>> GetShipmentItemsById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetShipmentItemsById = {id}");

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var shipment = await _shipmentItemRepo.GetAsync(d => d.ShipmentId == id);

                if (shipment == null)
                {
                    _logger.LogInformation($"{DateTime.Now} shipments with id {id} not found", id);
                    return NotFound();
                }

                
                var shipmentItems = await _shipmentItemRepo.GetAllAsync(x => x.ShipmentId == id, new List<string> { "Product" });

                if (shipmentItems == null)
                {
                    _logger.LogInformation($"{DateTime.Now} items for shipment with id {id}  not found", id);
                    return NotFound();

                }

                
                IEnumerable<GetShipmentItemDto> getShipmentItemsDto = shipmentItems.Select(d => _wrapper.Bind(d));

                return Ok(getShipmentItemsDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetShipmentItems by id ={id} exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
