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
    /// Shipments orders data reporting
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepo;
        private readonly ILogger<ShipmentController> _logger;
        private readonly IWMSwrapper _wrapper;

        /// <summary> 
        /// Initializes a new instance of the <see cref="ShipmentController" /> class.
        /// </summary>
        /// <param name="shipmentRepo">An instance of the shipment repository interface responsible for data access.</param>
        /// <param name="logger">An instance of the logger interface for logging controller actions and events.</param>
        /// <param name="wrapper">An instance of the Warehouse Management System (WMS) wrapper interface for integration.</param>
        public ShipmentController(IShipmentRepository shipmentRepo, ILogger<ShipmentController> logger, IWMSwrapper wrapper)
        {
            _shipmentRepo = shipmentRepo;
            _logger = logger;
            _wrapper = wrapper;
        }

        /// <summary>
        /// Retrieves a list of all shipments from the database.
        /// </summary>
        /// <returns>A collection containing all shipments in the database.</returns>
        /// <response code="200">OK: The request was successful, and all shipments are returned.</response>
        /// <response code="401">Unauthorized: The client could not authenticate the request.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpGet("/GetShipments", Name = "GetShipments")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetShipmentDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<GetShipmentDto>>> GetShipments()
        {
            _logger.LogInformation($"{DateTime.Now} Executed GetShipments.");

            try
            {
                var shipments = await _shipmentRepo.GetAllAsync(null, new List<string> { "ShipmentStatus", "RWMSuser" });

                IEnumerable<GetShipmentDto> getShipmentDto = shipments.Select(d => _wrapper.Bind(d)).ToList();

                return Ok(getShipmentDto);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} HttpGet GetShipments exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
