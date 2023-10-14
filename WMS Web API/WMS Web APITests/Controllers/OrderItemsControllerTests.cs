using WMS_Web_API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WMS.Domain.Models;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API.DTO;
using WMS_Web_API.API;
using Moq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Castle.Core.Resource;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class OrderItemsControllerTests
    {
        private OrderItemsController? _orderItemsController;
        private Mock<IOrderItemRepository>? _mockOrderItemRepo;
        private Mock<ILogger<OrderItemsController>>? _mockLogger;
        private Mock<IWMSwrapper>? _mockWrapper;

        [TestInitialize]
        public void OnInit()
        {
            _mockOrderItemRepo = new Mock<IOrderItemRepository>();
            _mockLogger = new Mock<ILogger<OrderItemsController>>();
            _mockWrapper = new Mock<IWMSwrapper>();
            _orderItemsController = new OrderItemsController(_mockOrderItemRepo.Object, _mockLogger.Object, _mockWrapper.Object);
        }

        [TestMethod()]
        public async Task Create_Item_Returns_Created_Item()
        {
            //Arrange
            CreateOrderItemDto reqItem = new CreateOrderItemDto()
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            OrderItem orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            _mockWrapper.Setup(x => x.Bind(It.IsAny<CreateOrderItemDto>())).Returns(orderItem);

            bool itemCreated = true;
            _mockOrderItemRepo?.Setup<Task<bool>>(repo => repo.CreateAsync(orderItem)).Returns(Task.FromResult(itemCreated));

            //Act
            var result = await _orderItemsController.Create(reqItem);

            //Assert
            var actualResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);

            var actualItem = actualResult.Value as CreateNewResourceResponseDto;
            Assert.IsNotNull(actualItem);
            Assert.AreEqual(orderItem.Id, actualItem.Id);
        }

        [TestMethod()]
        public async Task Create_Item_Returns_Bad_Request()
        {
            //Arrange
            CreateOrderItemDto reqItem = new CreateOrderItemDto()
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            OrderItem orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            _mockWrapper.Setup(x => x.Bind(It.IsAny<CreateOrderItemDto>())).Returns(orderItem);

            bool itemCreated = false;
            _mockOrderItemRepo?.Setup<Task<bool>>(repo => repo.CreateAsync(orderItem)).Returns(Task.FromResult(itemCreated));

            //Act
            var result = await _orderItemsController.Create(reqItem);

            //Assert
            var actualResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Create_Item_Returns_Internal_Server_Error()
        {
            //Arrange
            CreateOrderItemDto reqItem = new CreateOrderItemDto()
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            OrderItem orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };
            _mockWrapper.Setup(x => x.Bind(It.IsAny<CreateOrderItemDto>())).Returns(orderItem);

            _mockOrderItemRepo?.Setup<Task<bool>>(repo => repo.CreateAsync(orderItem)).Throws(new Exception());

            //Act
            var result = await _orderItemsController.Create(reqItem);

            //Assert
            var actualResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Delete_Item_Returns_No_Content()
        {
            //Arrange
            int id = 1;
            var orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync(orderItem);

            _mockOrderItemRepo?.Setup(repo => repo.RemoveAsync(orderItem)).Returns(Task.CompletedTask);

            //Act
            var result = await _orderItemsController.DeleteOrderItem(id);

            //Assert
            var actualResult = result as NoContentResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Delete_Item_Returns_Not_Found()
        {
            //Arrange
            int id = 1;

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync((OrderItem)null);

            //Act
            var result = await _orderItemsController.DeleteOrderItem(id);

            //Assert
            var actualResult = result as NotFoundResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Delete_Item_Returns_Internal_Server_Error()
        {
            //Arrange
            int id = 1;
            var orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                               It.IsAny<Expression<Func<OrderItem, bool>>>(),
                                              It.IsAny<bool>()))
                .ReturnsAsync(orderItem);

            _mockOrderItemRepo?.Setup(repo => repo.RemoveAsync(orderItem)).Throws(new Exception());

            //Act
            var result = await _orderItemsController.DeleteOrderItem(id);

            //Assert
            var actualResult = result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Update_Item_Returns_No_Content()
        {
            //Arrange
            int id = 1;
            UpdateOrderItemDto updateOrderItemDto = new UpdateOrderItemDto()
            {
                ProductId = 1,
                Quantity = 1
            };
            OrderItem orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                                              It.IsAny<Expression<Func<OrderItem, bool>>>(),
                                              It.IsAny<bool>()))
             .ReturnsAsync(orderItem);

            _mockWrapper.Setup(a => a.Bind(updateOrderItemDto, orderItem)).Returns(orderItem);
            _mockOrderItemRepo?.Setup(repo => repo.UpdateAsync(orderItem)).Returns(Task.CompletedTask);

            //Act
            var result = await _orderItemsController.UpdateOrderItem(id, updateOrderItemDto);

            //Assert
            var actualResult = result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Update_Item_Returns_Not_Found()
        {
            //Arrange
            int id = 1;
            UpdateOrderItemDto updateOrderItemDto = new UpdateOrderItemDto()
            {
                ProductId = 1,
                Quantity = 1
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                                                             It.IsAny<Expression<Func<OrderItem, bool>>>(),
                                                             It.IsAny<bool>()))
             .ReturnsAsync((OrderItem)null);

            //Act
            var result = await _orderItemsController.UpdateOrderItem(id, updateOrderItemDto);

            //Assert
            var actualResult = result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task Update_Item_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            //Arrange
            int id = 1;
            UpdateOrderItemDto updateOrderItemDto = new UpdateOrderItemDto()
            {
                ProductId = 1,
                Quantity = 1
            };
            OrderItem orderItem = new OrderItem()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                                                             It.IsAny<Expression<Func<OrderItem, bool>>>(),
                                                             It.IsAny<bool>()))
                                                            .ReturnsAsync(orderItem);

            _mockWrapper.Setup(a => a.Bind(updateOrderItemDto, orderItem)).Returns(orderItem);
            _mockOrderItemRepo?.Setup(repo => repo.UpdateAsync(orderItem)).Throws(new Exception());

            //Act
            var result = await _orderItemsController.UpdateOrderItem(id, updateOrderItemDto);

            //Assert
            var actualResult = result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetOrderItemById_Returns_Success_OrderItem()
        {
            // Arrange
            var id = 1;
            var orderItem = new OrderItem { Id = id };
            var orderItemDto = new GetOrderItemDto() { Id = id };
            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(), // Match any filter
                It.Is<ICollection<string>>(tables => tables.Contains("Product")), // Match the includeTables parameter
                It.IsAny<bool>())) // Match any value for the tracked parameter
                .ReturnsAsync(orderItem);

            _mockWrapper?.Setup(x => x.Bind(orderItem)).Returns(orderItemDto);
            
            // Act
            var result = await _orderItemsController.GetOrderItemById(id);

            // Assert
            var actualResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);

            var actualOrderItem = actualResult.Value as GetOrderItemDto;
            Assert.IsNotNull(actualOrderItem);
            Assert.AreEqual(orderItem.Id, actualOrderItem.Id);
        }

        [TestMethod()]
        public async Task GetOrderItemById_Returns_NotFound()
        {
            // Arrange
            var id = 1;
            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(), // Match any filter
                It.Is<ICollection<string>>(tables => tables.Contains("Product")), // Match the includeTables parameter
                It.IsAny<bool>())) // Match any value for the tracked parameter
                .ReturnsAsync((OrderItem)null);
                                                                              
            // Act
            var result = await _orderItemsController.GetOrderItemById(id);

           // Assert
           var actualResult = result.Result as NotFoundResult;
           Assert.IsNotNull(actualResult);
           Assert.AreEqual(StatusCodes.Status404NotFound, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetOrderItemById_Returns_InternalServerError()
        {
            // Arrange
            var id = 1;
            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.Is<ICollection<string>>(tables => tables.Contains("Product")),
                It.IsAny<bool>()))
            .Throws(new Exception());
                                                                              
            // Act
            var result = await _orderItemsController.GetOrderItemById(id);

            // Assert
            var actualResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, actualResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetOrderItems_Returns_Success_OrderItems()
        {
            // Arrange
            var id = 1;
            var orderItem =
                new OrderItem { Id = 1 , OrderId = 1};
             
            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1 , OrderId = 1},
                new OrderItem { Id = 2 , OrderId = 1}
            };
            var orderItemDtos = new List<GetOrderItemDto>
            {
                new GetOrderItemDto { Id = 1 },
                new GetOrderItemDto { Id = 2 }
            };

            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(orderItem);

            _mockOrderItemRepo?.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.Is<ICollection<string>>(tables => tables.Contains("Product"))
                ))
                .ReturnsAsync(orderItems);
           
            _mockWrapper?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<OrderItem>())).Returns(orderItemDtos[0]).Returns(orderItemDtos[1]);

            // Act
            var result = await _orderItemsController.GetOrderItemsById(id);

            // Assert
            var actualResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);

            var actualOrderItems = actualResult.Value as IEnumerable<GetOrderItemDto>;
            Assert.IsNotNull(actualOrderItems);
            Assert.AreEqual(orderItems.Count, actualOrderItems.Count());
        }

        [TestMethod]
        public async Task GetOrderItems_Returns_NotFound()
        {
            // Arrange
            var id = 1;
            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync((OrderItem)null);

            // Act
            var result = await _orderItemsController.GetOrderItemsById(id);

            // Assert
            var actualResult = result.Result as NotFoundResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, actualResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrderItems_Returns_InternalServerError()
        {
            // Arrange
            var id = 1;
            _mockOrderItemRepo?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<OrderItem, bool>>>(),
                It.IsAny<bool>()))
            .Throws(new Exception());

            // Act
            var result = await _orderItemsController.GetOrderItemsById(id);

            // Assert
            var actualResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, actualResult.StatusCode);
        }
    }
}