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
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepo;
        private readonly ILogger<ShipmentController> _logger;
        private readonly IWMSwrapper _wrapper;

        public ShipmentController(IShipmentRepository shipmentRepo, ILogger<ShipmentController> logger, IWMSwrapper wrapper)
        {
            _shipmentRepo = shipmentRepo;
            _logger = logger;
            _wrapper = wrapper;
        }

        /// <summary>
        /// Fetches all shipments
        /// </summary>
        /// <returns>All shipments in DB</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Client could not authenticate a request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetShipments")]
        //     [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetShipmentDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetShipmentDto>>> GetShipments()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetShipments.");

            try
            {
                var shipments = await _shipmentRepo.GetAllAsync();


                IEnumerable<GetShipmentDto> getShipmentDto = shipments.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getShipmentDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetShipments exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
        /// <remarks>
        /// Sample request:
        ///
        ///     GET Shipment/1
        ///     {
        ///     }
        ///
        /// </remarks>
        [HttpGet("{id:int}/Items", Name = "GetShipmentItemsById")]
        //      [Authorize]
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

                var shipment = await _shipmentRepo.GetAsync(d => d.Id == id);

                if (shipment == null)
                {
                    _logger.LogInformation($"{DateTime.Now} shipments with id {id} not found", id);
                    return NotFound();
                }

                var shipmentItems = await _shipmentRepo.GetShipmentItemsByIdAsync(id);

                IEnumerable<GetShipmentItemDto> getShipmentItemsDto = shipmentItems.Select(d => _wrapper.Bind(d)).ToList();

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
