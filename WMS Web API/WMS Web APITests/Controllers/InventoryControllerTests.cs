using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS_Web_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Infastructure.Interfaces;
using Microsoft.Extensions.Logging;
using WMS_Web_API.API;
using Moq;
using WMS.Infastructure.Repositories;
using WMS_Web_API.API.DTO;
using WMS.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class InventoryControllerTests
    {
        private InventoryController? _inventoryController;
        private Mock<IInventoryRepository>? _inventoryRepoMock;
        private Mock<ILogger<InventoryController>>? _loggerMock;
        private Mock<IWMSwrapper>? _wrapperMock;

        [TestInitialize]
        public void Setup()
        {
            _inventoryRepoMock = new Mock<IInventoryRepository>();
            _loggerMock = new Mock<ILogger<InventoryController>>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _inventoryController = new InventoryController(_inventoryRepoMock.Object, _loggerMock.Object, _wrapperMock.Object);
        }

        [TestMethod()]
        public async Task GetInventoriesTest()
        {
            //Arrange
            var inventory = new List<Inventory> { 
                new Inventory{ Id = 1, WarehouseId = 1, ProductId = 1, Quantity = 1},
                new Inventory{ Id = 2, WarehouseId = 1, ProductId = 2, Quantity = 1}
            };
            
            _inventoryRepoMock.Setup(x => x.GetAllAsync(null, new List<string> { "Warehouse", "Product" })).ReturnsAsync(inventory);

            var getInventoryDtos = new List<GetInventoryDto>
            {
                new GetInventoryDto{ Id = 1, ProductDescription = "test1", ProductName = "test1", ProductSKU = "001", Quantity = 1, WarehouseName = "test" },
                new GetInventoryDto{ Id = 2, ProductDescription = "test2", ProductName = "test2", ProductSKU = "002", Quantity = 1, WarehouseName = "test" }
            };

            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<Inventory>())).Returns(getInventoryDtos[0]).Returns(getInventoryDtos[1]);

            //Act
            var result = await _inventoryController.GetInventories();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var actualInventory = okResult.Value as IEnumerable<GetInventoryDto>;
            Assert.IsNotNull(actualInventory);
            Assert.AreEqual(inventory.Count(), actualInventory.Count());

            Assert.AreEqual(inventory[0].Id, actualInventory.ElementAt(0).Id);
            Assert.AreEqual(inventory[1].Id, actualInventory.ElementAt(1).Id);

            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<GetInventoryDto>>));
        }

        [TestMethod()]
        public async Task GetWarehousesRatioOfOccupiedTest()
        {
            //Arrange
            var warehouses = new List<Warehouse>
            {
                new Warehouse{ Id = 1, Name = "test1", Description = "test1", Location = "test1", TotalVolume = 1, TotalWeigth = 1},
                new Warehouse{ Id = 2, Name = "test2", Description = "test2", Location = "test2", TotalVolume = 1, TotalWeigth = 1}
            };

            _inventoryRepoMock.Setup(x => x.GetWarehouseListAsync()).ReturnsAsync(warehouses);
            
            //Act
            var result = await _inventoryController.GetWarehousesRatioOfOccupied();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var actualWarehouses = okResult.Value as IEnumerable<GetWarehousesRatioOfOccupiedDto>;
            Assert.IsNotNull(actualWarehouses);
            Assert.AreEqual(warehouses.Count(), actualWarehouses.Count());

        }

        [TestMethod()]
        public async Task GetInventories_Returns_Status500InternalServerError()
        {
            //Arrange
            _inventoryRepoMock.Setup(repo => repo.GetAllAsync(null, new List<string> { "Warehouse", "Product" })).ThrowsAsync(new Exception());

            //Act
            var result = await _inventoryController.GetInventories();

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetWarehousesRatioOfOccupied_Returns_Status500InternalServerError()
        {
            //Arrange
            _inventoryRepoMock.Setup(repo => repo.GetWarehouseListAsync()).ThrowsAsync(new Exception());

            //Act
            var result = await _inventoryController.GetWarehousesRatioOfOccupied();

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}