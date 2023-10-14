using WMS_Web_API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;
using Castle.Core.Resource;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing;
using System.Net.Mime;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class OrderControllerTests
    {
        private OrderController? _orderController;
        private Mock<IOrderRepository>? _orderRepoMock;
        private Mock<IWMSwrapper>? _wrapperMock;
        private Mock<ILogger<OrderController>>? _loggerMock;
        private Mock<IInventoryManagerService>? _inventoryManagerServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _loggerMock = new Mock<ILogger<OrderController>>();
            _inventoryManagerServiceMock = new Mock<IInventoryManagerService>();
            _orderController = new OrderController(_orderRepoMock.Object, _loggerMock.Object, _wrapperMock.Object, _inventoryManagerServiceMock.Object);
        }

        [TestMethod]
        public async Task GetOrders_Returns_OK_Result_With_Order_Dtos()
        {
            // Arrange
            var orders = new List<Order> {
                new Order{ Id = 1, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1},
                new Order()
            };

            _orderRepoMock.Setup(repo => repo.GetAllAsync(null, new List<string> { "OrderStatus", "OrderType", "Customer", "RWMSuser" }))
                .ReturnsAsync(orders);

            var expectedGetOrderDto = orders.Select(o => _wrapperMock.Object.Bind(o));

            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<Order>())).Returns(expectedGetOrderDto.ElementAt(0)).Returns(expectedGetOrderDto.ElementAt(1));

            // Act
            var result = await _orderController.GetOrders();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);


            var actualOrderDto = okResult.Value as IEnumerable<GetOrderDto>;
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<GetOrderDto>));

            CollectionAssert.AreEqual(expectedGetOrderDto.ToList(), actualOrderDto.ToList());
            Assert.AreEqual(orders.Count(), actualOrderDto.Count());
        }

        [TestMethod]
        public async Task GetOrders_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            _orderRepoMock.Setup(repo => repo.GetAllAsync(null, new List<string> { "OrderStatus", "OrderType", "Customer", "RWMSuser" }))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.GetOrders();

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateNewOrder_Returns_201_When_Successful()
        {
            // Arrange

            var createOrderDto = new CreateOrderDto { CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            var order = new Order { Id = 1, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            var createNewResourceResponseDto = new CreateNewResourceResponseDto() { Id = 1 };

            _wrapperMock?.Setup(a => a.Bind(createOrderDto)).Returns(order);
            _wrapperMock?.Setup(a => a.Bind(order.Id)).Returns(createNewResourceResponseDto);
            
            bool orderCreated = true;
            _orderRepoMock?.Setup<Task<bool>>(repo => repo.CreateAsync(order)).Returns(Task.FromResult(orderCreated));

            // Act
            var result = await _orderController.Create(createOrderDto);

            // Assert
            var actualNewOrder = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actualNewOrder);
            Assert.AreEqual(StatusCodes.Status201Created, actualNewOrder.StatusCode);

            var newOrderDto = actualNewOrder.Value as CreateNewResourceResponseDto;
            Assert.IsNotNull(newOrderDto);
            Assert.AreEqual(1, newOrderDto.Id);
        }

        [TestMethod]
        public async Task CreateNewOrder_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto { CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            var order = new Order { Id = 1, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };

            _wrapperMock?.Setup(a => a.Bind(createOrderDto)).Returns(order);
            _orderRepoMock?.Setup<Task<bool>>(repo => repo.CreateAsync(order)).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.Create(createOrderDto);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateOrder_Returns_Updated_204_When_Successful()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto { OrderStatusId = 1, OrderTypeId = 1};

            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync(order);

            _wrapperMock?.Setup(a => a.Bind(updateOrderDto, order)).Returns(order);
            _orderRepoMock?.Setup(repo => repo.UpdateAsync(order)).Returns(Task.CompletedTask);

            // Act
            var result = await _orderController.UpdateOrder( orderID , updateOrderDto);

            // Assert
            var actualUpdatedOrder = result as NoContentResult;
            Assert.IsNotNull(actualUpdatedOrder);
            Assert.AreEqual(StatusCodes.Status204NoContent, actualUpdatedOrder.StatusCode);
        }

        [TestMethod]
        public async Task UpdateOrder_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto { OrderStatusId = 1, OrderTypeId = 1 };

            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync(order);
            _wrapperMock?.Setup(a => a.Bind(updateOrderDto, order)).Returns(order);
            _orderRepoMock?.Setup(repo => repo.UpdateAsync(order)).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.UpdateOrder(orderID, updateOrderDto);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateOrder_Returns_Not_Found_404_When_Order_Is_Not_Found()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto { OrderStatusId = 1, OrderTypeId = 1 };

            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync((Order)null);
            _wrapperMock?.Setup(a => a.Bind(updateOrderDto, order)).Returns(order);
            _orderRepoMock?.Setup(repo => repo.UpdateAsync(order)).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.UpdateOrder(orderID, updateOrderDto);

            // Assert
            var statusCodeResult = result as NotFoundResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOrder_Returns_Deleted_204_When_Successful()
        {
            // Arrange
            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync(order);
            _orderRepoMock?.Setup(repo => repo.RemoveAsync(order)).Returns(Task.CompletedTask);

            // Act
            var result = await _orderController.DeleteOrder(orderID);

            // Assert
            var actualDeletedOrder = result as NoContentResult;
            Assert.IsNotNull(actualDeletedOrder);
            Assert.AreEqual(StatusCodes.Status204NoContent, actualDeletedOrder.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOrder_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync(order);
            _orderRepoMock?.Setup(repo => repo.RemoveAsync(order)).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.DeleteOrder(orderID);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOrder_Returns_Not_Found_404_When_Order_Is_Not_Found()
        {
            // Arrange
            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };
            _orderRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync((Order)null);
            _orderRepoMock?.Setup(repo => repo.RemoveAsync(order)).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.DeleteOrder(orderID);

            // Assert
            var statusCodeResult = result as NotFoundResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrder_Returns_OK_Result_With_Order_Dto_When_Order_Is_Found()
        {
            // Arrange
            int orderID = 1;
            var order = new Order { Id = orderID, CustomerId = 1, OrderStatusId = 1, OrderTypeId = 1, WMSuserId = 1 };

            _orderRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(order);
            var expectedGetOrderDto = new
                GetOrderDto { Id = orderID };

            _wrapperMock?.Setup(wrapper => wrapper.Bind(It.IsAny<Order>())).Returns(expectedGetOrderDto);

            // Act
            var result = await _orderController.GetOrderById(orderID);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrder_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            int orderID = 1;
            _orderRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.GetOrderById(orderID);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrder_Returns_Not_Found_404_When_Order_Is_Not_Found()
        {
            // Arrange
            int orderID = 1;
            _orderRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync((Order)null);

            // Act
            var result = await _orderController.GetOrderById(orderID);

            // Assert
            var statusCodeResult = result.Result as NotFoundResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Submmit_Order_Return_Ok()
        {
            // Arrange
            int orderID = 1;
            
            bool IsOrderTranfered = true;
            _inventoryManagerServiceMock.Setup(repo => repo.ProcessOrderAsync(orderID)).ReturnsAsync(IsOrderTranfered);

            // Act
            var result = await _orderController.SubmitOrder(orderID);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task Submmit_Order_Return_Bad_Request_When_Order_Is_Not_Tranfered()
        {
            // Arrange
            int orderID = 1;
            
            _inventoryManagerServiceMock.Setup(repo => repo.ProcessOrderAsync(orderID)).ReturnsAsync(false);
            // Act
            var result = await _orderController.SubmitOrder(orderID);

            // Assert
            var statusCodeResult = result.Result as BadRequestResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Submmit_Order_Return_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            int orderID = 1;            

            _inventoryManagerServiceMock.Setup(repo => repo.ProcessOrderAsync(orderID)).ThrowsAsync(new Exception());
            // Act
            var result = await _orderController.SubmitOrder(orderID);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrderStatuses_Returns_OK_Result_With_OrderStatus_Dtos()
        {
            // Arrange
            var orderStatuses = new List<OrderStatus>
            {
                new OrderStatus{ Id = 1, Name = "New"},
                new OrderStatus{ Id = 2, Name = "Completed"}
            };

            _orderRepoMock.Setup(repo => repo.GetOrderStatusListAsync())
                .ReturnsAsync(orderStatuses);

            var expectedGetOrderStatusDto = orderStatuses.Select(o => _wrapperMock.Object.Bind(o));

            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<OrderStatus>())).Returns(expectedGetOrderStatusDto.ElementAt(0)).Returns(expectedGetOrderStatusDto.ElementAt(1));

            // Act
            var result = await _orderController.GetOrderStatuses();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrderStatuses_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            _orderRepoMock.Setup(repo => repo.GetOrderStatusListAsync())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.GetOrderStatuses();

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrderTypes_Returns_OK_Result_With_OrderType_Dtos()
        {
            // Arrange
            var orderTypes = new List<OrderType>
            {
                new OrderType{ Id = 1, Name = "Inbound"},
                new OrderType{ Id = 2, Name = "Outbound"}
            };

            _orderRepoMock.Setup(repo => repo.GetOrderTypesListAsync())
                .ReturnsAsync(orderTypes);

            var expectedGetOrderTypeDto = orderTypes.Select(o => _wrapperMock.Object.Bind(o));

            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<OrderType>())).Returns(expectedGetOrderTypeDto.ElementAt(0)).Returns(expectedGetOrderTypeDto.ElementAt(1));

            // Act
            var result = await _orderController.GetOrderTypes();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(expectedGetOrderTypeDto.Count(), (okResult.Value as IEnumerable<GetOrderTypesDto>).Count());
        }

        [TestMethod]
        public async Task GetOrderTypes_Returns_Internal_Server_Error_Result_When_Exception_Is_Thrown()
        {
            // Arrange
            _orderRepoMock.Setup(repo => repo.GetOrderTypesListAsync())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _orderController.GetOrderTypes();

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}