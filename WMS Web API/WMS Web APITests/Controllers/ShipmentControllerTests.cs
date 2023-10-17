using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS_Web_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Infastructure.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;
using WMS_Web_API.API;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class ShipmentControllerTests
    {
        private ShipmentController? _shipmentController;
        private Mock<IShipmentRepository>? _shipmentRepoMock;
        private Mock<ILogger<ShipmentController>>? _loggerMock;
        private Mock<IWMSwrapper>? _wrapperMock;

        [TestInitialize]
        public void OnInit()
        {
            _shipmentRepoMock = new Mock<IShipmentRepository>();
            _loggerMock = new Mock<ILogger<ShipmentController>>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _shipmentController = new ShipmentController(_shipmentRepoMock.Object, _loggerMock.Object, _wrapperMock.Object);
        }

        [TestMethod()]
        public async Task GetShipments_Returns_OkResultWithListOfShipments()
        {
            //Arrange
            var shipments = new List<Shipment>
            {
                            new Shipment{ Id=1, OrderId = 1, CustomerId = 1},
                            new Shipment{ Id=2, OrderId = 2, CustomerId = 2},
                            new Shipment{ Id=3, OrderId = 3, CustomerId = 3},
            };

            var expectedShipments = shipments.Select(p => new GetShipmentDto { Id = p.Id,  OrderId = p.OrderId});

            _shipmentRepoMock?.Setup(repo => repo.GetAllAsync(null, new List<string> { "ShipmentStatus", "RWMSuser" })).ReturnsAsync(shipments);
            
            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<Shipment>())).Returns(expectedShipments.ElementAt(0)).Returns(expectedShipments.ElementAt(1)).Returns(expectedShipments.ElementAt(2));

            //Act
            var result = await _shipmentController?.GetShipments();

            //Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var results = okResult.Value as IEnumerable<GetShipmentDto>;
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedShipments.Count(), results.Count());
            for (int i = 0; i < results.Count(); i++)
            {
                Assert.AreEqual(expectedShipments.ElementAt(i).Id, results.ElementAt(i).Id);
                Assert.AreEqual(expectedShipments.ElementAt(i).OrderId, results.ElementAt(i).OrderId);
            }
        }

        [TestMethod]
        public async Task GetShipments_Returns_Status500InternalServerError()
        {
            //Arrange
            _shipmentRepoMock?.Setup(repo => repo.GetAllAsync(null, new List<string> { "ShipmentStatus", "RWMSuser" })).ThrowsAsync(new Exception());

            //Act
            var result = await _shipmentController?.GetShipments();

            //Assert
            Assert.IsNotNull(result);
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}