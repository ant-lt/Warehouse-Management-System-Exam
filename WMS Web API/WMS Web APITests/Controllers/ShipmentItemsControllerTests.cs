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
using System.Linq.Expressions;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class ShipmentItemsControllerTests
    {
        private ShipmentItemsController? _shipmentItemsController;
        private Mock<IShipmentRepository>? _shipmentRepoMock;
        private Mock<IShipmentItemRepository>? _shipmentItemRepoMock;
        private Mock<ILogger<ShipmentItemsController>>? _loggerMock;
        private Mock<IWMSwrapper>? _wrapperMock;

        [TestInitialize]
        public void OnInit()
        {
            _shipmentRepoMock = new Mock<IShipmentRepository>();
            _shipmentItemRepoMock = new Mock<IShipmentItemRepository>();
            _loggerMock = new Mock<ILogger<ShipmentItemsController>>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _shipmentItemsController = new ShipmentItemsController(_shipmentItemRepoMock.Object, _shipmentRepoMock.Object, _loggerMock.Object, _wrapperMock.Object);
        }

        [TestMethod()]
        public async Task GetShipmentItemsById_WithValidId_ReturnsOkResult()
        {
           // Arrange
            int shipmentId = 1;
            var shipment = new Shipment { Id = shipmentId, OrderId = 1, CustomerId = 1 };
            var shipmentItems = new List<ShipmentItem>
            {
                new ShipmentItem{ Id=1, ShipmentId = 1, ProductId = 1, Quantity = 1},
                new ShipmentItem{ Id=2, ShipmentId = 1, ProductId = 2, Quantity = 2},
                new ShipmentItem{ Id=3, ShipmentId = 1, ProductId = 3, Quantity = 3},
            };
            // var expectedShipmentItemsDto = shipmentItems.Select(p => new GetShipmentItemDto { Id = p.Id, ShipmentId = p.ShipmentId,  Quantity = p.Quantity });

            IEnumerable<GetShipmentItemDto> expectedShipmentItemsDto = new List<GetShipmentItemDto>
            {
                new GetShipmentItemDto{ Id=1, ShipmentId = 1, Quantity = 1},
                new GetShipmentItemDto{ Id=2, ShipmentId = 1, Quantity = 2},
                new GetShipmentItemDto{ Id=3, ShipmentId = 1, Quantity = 3},
            };

            _shipmentRepoMock?.Setup(repo => repo.GetAsync(
                                    It.IsAny<Expression<Func<Shipment, bool>>>(),
                                    It.IsAny<bool>()))
            .ReturnsAsync(shipment);
            _shipmentItemRepoMock?.Setup(repo => repo.GetAllAsync(d => d.ShipmentId == shipmentId, new List<string> { "Product" })).ReturnsAsync(shipmentItems);
            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<ShipmentItem>())).Returns(expectedShipmentItemsDto.ElementAt(0)).Returns(expectedShipmentItemsDto.ElementAt(1)).Returns(expectedShipmentItemsDto.ElementAt(2));
            
            // Act
            var result = await _shipmentItemsController?.GetShipmentItemsById(shipmentId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var results = okResult.Value as IEnumerable<GetShipmentItemDto>;
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedShipmentItemsDto.Count(), results.Count());
        }

        [TestMethod()]
        public async Task GetShipmentItems_No_Shipment_ReturnNoFound()
        {
            // Arrange
            int shipmentId = 1;
            Shipment? shipment = null;

            _shipmentRepoMock?.Setup(repo => repo.GetAsync(
                        It.IsAny<Expression<Func<Shipment, bool>>>(),
                        It.IsAny<bool>()))
            .ReturnsAsync(shipment);

            // Act
            var result = await _shipmentItemsController?.GetShipmentItemsById(shipmentId);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        [TestMethod()]
        public async Task GetShipmentItems_No_ShipmentItems_ReturnNoFound()
        {
            // Arrange
            int shipmentId = 1;
            var shipment = new Shipment { Id = shipmentId, OrderId = 1, CustomerId = 1 };
            List<ShipmentItem>? shipmentItems = null;

            _shipmentRepoMock?.Setup(repo => repo.GetAsync(
                                       It.IsAny<Expression<Func<Shipment, bool>>>(),
                                                              It.IsAny<bool>()))
            .ReturnsAsync(shipment);
            _shipmentItemRepoMock?.Setup(repo => repo.GetAllAsync(d => d.ShipmentId == shipmentId, new List<string> { "Product" })).ReturnsAsync(shipmentItems);

            // Act
            var result = await _shipmentItemsController?.GetShipmentItemsById(shipmentId);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        [TestMethod()]
        public async Task GetShipmentItems_Returns_Status500InternalServerError()
        {
            //Arrange
            int shipmentId = 1;
            var shipment = new Shipment { Id = shipmentId, OrderId = 1, CustomerId = 1 };
            var shipmentItems = new List<ShipmentItem>
            {
                new ShipmentItem{ Id=1, ShipmentId = 1, ProductId = 1, Quantity = 1},
                new ShipmentItem{ Id=2, ShipmentId = 1, ProductId = 2, Quantity = 2},
                new ShipmentItem{ Id=3, ShipmentId = 1, ProductId = 3, Quantity = 3},
            };
            // var expectedShipmentItemsDto = shipmentItems.Select(p => new GetShipmentItemDto { Id = p.Id, ShipmentId = p.ShipmentId,  Quantity = p.Quantity });

            IEnumerable<GetShipmentItemDto> expectedShipmentItemsDto = new List<GetShipmentItemDto>
            {
                new GetShipmentItemDto{ Id=1, ShipmentId = 1, Quantity = 1},
                new GetShipmentItemDto{ Id=2, ShipmentId = 1, Quantity = 2},
                new GetShipmentItemDto{ Id=3, ShipmentId = 1, Quantity = 3},
            };

            _shipmentRepoMock?.Setup(repo => repo.GetAsync(
                                                   It.IsAny<Expression<Func<Shipment, bool>>>(),
                                                                                      It.IsAny<bool>()))
            .ThrowsAsync(new Exception());

            // Act
            var result = await _shipmentItemsController?.GetShipmentItemsById(shipmentId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, okResult.StatusCode);

        }
    }
}