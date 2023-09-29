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

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class CustomerControllerTests
    {
        private CustomerController? _customerController;
        private Mock<ICustomerRepository>? _customerRepoMock;
        private Mock<ILogger<CustomerController>>? _loggerMock;
        private Mock<IWMSwrapper>? _wrapperMock;

        [TestInitialize]
        public void OnInit()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<CustomerController>>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _customerController = new CustomerController(_customerRepoMock.Object, _loggerMock.Object, _wrapperMock.Object);
        }

        [TestMethod]
        public async Task GetCustomers_Returns_OkResultWithListOfCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer> 
            {
                new Customer { Id = 1, Name = "Customer 1", Address = "adress line" , City = "city" , ContactPerson = "contact person", Country = "LT" , Email ="test@test" , LegalCode ="1255", PhoneNumber = "54545454", PostCode = "2555", Status = true},
                new Customer { Id = 2, Name = "Customer 2", Address =  "adress line" , City = "city" , ContactPerson = "contact person", Country = "LT" , Email ="test@test" , LegalCode ="1255", PhoneNumber = "54545454", PostCode = "2555", Status = true}
            };
            _customerRepoMock?.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(expectedCustomers);

            var getCustomerDtos = new List<GetCustomerDto> 
            { 
                new GetCustomerDto { Id = 1, Name = "Customer 1" },
                new GetCustomerDto { Id = 2, Name = "Customer 2" }
            };
            //_wrapperMock?.Setup(wrapper => wrapper.Bind(It.IsAny<Customer>())).Returns(getCustomerDtos[0]);
            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<Customer>())).Returns(getCustomerDtos[0]).Returns(getCustomerDtos[1]);

            // Act
            var result = await _customerController.GetCustomers();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var actualCustomers = okResult.Value as List<GetCustomerDto>;
            
            Assert.IsNotNull(actualCustomers);
            Assert.AreEqual(expectedCustomers.Count, actualCustomers.Count);

            Assert.AreEqual(expectedCustomers[0].Id, actualCustomers[0].Id);
            Assert.AreEqual(expectedCustomers[0].Name, actualCustomers[0].Name);

            Assert.AreEqual(expectedCustomers[1].Id, actualCustomers[1].Id);
            Assert.AreEqual(expectedCustomers[1].Name, actualCustomers[1].Name);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            // Arrange
            _customerRepoMock?.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception());

            // Act
            var result = await _customerController.GetCustomers();

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };
            var getCustomerDto = new GetCustomerDto() { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };

            _wrapperMock?.Setup(a => a.Bind(customer)).Returns(getCustomerDto);
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Customer, bool>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerController.GetCustomerById(1);

            // Assert
            var actualCustomer = result.Result as OkObjectResult;
            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(StatusCodes.Status200OK, actualCustomer.StatusCode);

            var customerDto = actualCustomer.Value as GetCustomerDto;
            Assert.IsNotNull(customerDto);
            Assert.AreEqual(1, customerDto.Id);
            Assert.AreEqual("Customer 1", customerDto.Name);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsNotFoundResult_When_Customer_Is_Not_Found()
        {
            // Arrange
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                               It.IsAny<Expression<Func<Customer, bool>>>(),
                               It.IsAny<bool>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerController.GetCustomerById(1);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            // Arrange
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Customer, bool>>>(),
                It.IsAny<bool>()))
                .Throws(new Exception());

            // Act
            var result = await _customerController.GetCustomerById(1);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateCustomer_ReturnsCreatedCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };
            var createCustomerDto = new CreateCustomerDto() { Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555" };
            var createNewResourceResponseDto = new CreateNewResourceResponseDto() { Id = 1 };

     
            _wrapperMock?.Setup(a => a.Bind(createCustomerDto)).Returns(customer);
            _wrapperMock?.Setup(a => a.Bind(customer.Id)).Returns(createNewResourceResponseDto);
            bool customerCreated = true;

            _customerRepoMock?.Setup<Task<bool>>(repo => repo.CreateAsync(customer)).Returns(Task.FromResult(customerCreated));

            // Act
            var result = await _customerController.Create(createCustomerDto);

            // Assert
            var actualNewCustomer = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actualNewCustomer);
            Assert.AreEqual(StatusCodes.Status201Created, actualNewCustomer.StatusCode);
            
            var customerDto = actualNewCustomer.Value as CreateNewResourceResponseDto;
            Assert.IsNotNull(customerDto);
            Assert.AreEqual(1, customerDto.Id);            
        }

        [TestMethod]
        public async Task CreateCustomer_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            // Arrange
            var createCustomerDto = new CreateCustomerDto() { Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555" };
            _wrapperMock?.Setup(a => a.Bind(createCustomerDto)).Throws(new Exception());

            // Act
            var result = await _customerController.Create(createCustomerDto);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_ReturnsUpdatedCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };
            var updateCustomerDto = new UpdateCustomerDto() { Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555" };

            _customerRepoMock?.Setup(repo => repo.GetAsync(
                                              It.IsAny<Expression<Func<Customer, bool>>>(),
                                              It.IsAny<bool>()))
                                              .ReturnsAsync(customer);
            _wrapperMock?.Setup(a => a.Bind(updateCustomerDto, customer)).Returns(customer);
            _customerRepoMock?.Setup(repo => repo.UpdateAsync(customer)).Returns(Task.CompletedTask);
      

            // Act
            var result = await _customerController.UpdateCustomer(1, updateCustomerDto);

            // Assert            
            var statusCodeResult = result as NoContentResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_ReturnsNotFoundResult_When_Customer_Is_Not_Found()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto() { Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555" };
            _customerRepoMock?.Setup(
                repo => repo.GetAsync(
                        It.IsAny<Expression<Func<Customer, bool>>>(),
                        It.IsAny<bool>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerController.UpdateCustomer(1, updateCustomerDto);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };
            var updateCustomerDto = new UpdateCustomerDto() { Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555" };
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                        It.IsAny<Expression<Func<Customer, bool>>>(),
                        It.IsAny<bool>()))
                .Throws(new Exception());
            _wrapperMock?.Setup(a => a.Bind(updateCustomerDto, customer)).Throws(new Exception());
            // Act
            var result = await _customerController.UpdateCustomer(1, updateCustomerDto);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsNoContentResult()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };

            _customerRepoMock?.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(customer);
            _customerRepoMock?.Setup(repo => repo.RemoveAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _customerController.DeleteCustomer(1);

            // Assert
            var actualCustomer = result as NoContentResult;
            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(204, actualCustomer.StatusCode);
        }
        
        [TestMethod]
        public async Task DeleteCustomer_ReturnsNotFoundResult_When_Customer_Is_Not_Found()
        {
            // Arrange
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<bool>()))
                    .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerController.DeleteCustomer(1);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsInternalServerErrorResult_When_Exception_Is_Thrown()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer 1", Address = "adress line", City = "city", ContactPerson = "contact person", Country = "LT", Email = "test@test", LegalCode = "1255", PhoneNumber = "54545454", PostCode = "2555", Status = true };
            _customerRepoMock?.Setup(repo => repo.GetAsync(
                                   It.IsAny<Expression<Func<Customer, bool>>>(),
                                                      It.IsAny<bool>()))
                .Throws(new Exception());
            _customerRepoMock?.Setup(repo => repo.RemoveAsync(customer)).Throws(new Exception());

            // Act
            var result = await _customerController.DeleteCustomer(1);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}