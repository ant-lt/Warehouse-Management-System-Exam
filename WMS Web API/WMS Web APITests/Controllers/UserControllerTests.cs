using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WMS.Infastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        private UserController? _userController;
        private Mock<IUserRepository>? _userRepoMock;
        private Mock<ILogger<UserController>>? _loggerMock;
        private Mock<IJwtService>? _jwtServiceMock;
        private Mock<IPasswordService>? _passwordServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _jwtServiceMock = new Mock<IJwtService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _userController = new UserController(_userRepoMock.Object, _loggerMock.Object, _jwtServiceMock.Object, _passwordServiceMock.Object);
        }

        [TestMethod()]
        public async Task Login_ReturnsOkWithValidCredentials()
        {
            // Arrange
            var username = "validUsername";
            var password = "validPassword";
            var token = "validToken";
            var loginUser = new WMSuser { Username = username, Role = new Role { Name = "Customer" }, Active = true };

            _jwtServiceMock.Setup(jwt => jwt.GetJwtToken(loginUser.Id, loginUser.Role.Name)).Returns(token);          
            _userRepoMock.Setup(repo => repo.LoginAsync(username, password)).ReturnsAsync(loginUser);

            var loginData = new LoginRequest { Username = username, Password = password };
           
            // Act
            var result = await _userController.Login(loginData);

            // Assert
            var okResult = result.Result as OkObjectResult;
               
            
            Assert.IsNotNull(result);
            
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var loginResponse = okResult.Value as LoginResponse;
            Assert.AreEqual(username, loginResponse.UserName);
            Assert.IsTrue(loginResponse.Active);
            Assert.AreEqual("Customer", loginResponse.Role);

        }

        [TestMethod()]
        public async Task Register_ReturnsCreatedWithValidData()
        {
            // Arrange
            var registrationData = new RegistrationRequest
            {
                Username = "newUsername",
                Password = "newPassword",
                Name = "New User",
                Role = "Customer"
            };
            var newUser = new WMSuser { Username = registrationData.Username, Id = 1, Role = new Role { Name = registrationData.Role }, Active = true };

            _userRepoMock.Setup(repo => repo.IsUniqueUserAsync(registrationData.Username)).ReturnsAsync(true);
            _userRepoMock.Setup(repo => repo.GetRolebyNameAsync(registrationData.Role)).ReturnsAsync(new Role { Name = registrationData.Role });
            _userRepoMock.Setup(repo => repo.RegisterAsync(It.IsAny<WMSuser>())).ReturnsAsync(true);

            // Act
            var result = await _userController.Register(registrationData);

            // Assert
            Assert.IsNotNull(result);
            var newUserRegistered = result.Result as CreatedAtActionResult;
            Assert.AreEqual(StatusCodes.Status201Created, newUserRegistered.StatusCode);
            var createNewResourceResponseDto = newUserRegistered.Value as CreateNewResourceResponseDto;
               
            Assert.IsNotNull(createNewResourceResponseDto);
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorizedWithInvalidCredentials()
        {
            // Arrange
            var username = "invalidUsername";
            var password = "invalidPassword";
            var loginUser = null as WMSuser;

            _userRepoMock.Setup(repo => repo.LoginAsync(username, password)).ReturnsAsync(loginUser);

            var loginData = new LoginRequest { Username = username, Password = password };

            // Act
            var result = await _userController.Login(loginData);

            // Assert
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public async Task Login_ReturnsBadRequestWithInactiveUser()
        {
            // Arrange
            var username = "inactiveUsername";
            var password = "inactivePassword";
            var loginUser = new WMSuser { Username = username, Role = new Role { Name = "Customer" }, Active = false };

            _userRepoMock.Setup(repo => repo.LoginAsync(username, password)).ReturnsAsync(loginUser);

            var loginData = new LoginRequest { Username = username, Password = password };

            // Act
            var result = await _userController.Login(loginData);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequestWithExistingUsername()
        {
            // Arrange
            var registrationData = new RegistrationRequest
            {
                Username = "existingUsername",
                Password = "newPassword",
                Name = "New User",
                Role = "Customer"
            };
            var newUser = new WMSuser { Username = registrationData.Username, Id = 1, Role = new Role { Name = registrationData.Role }, Active = true };

            _userRepoMock.Setup(repo => repo.IsUniqueUserAsync(registrationData.Username)).ReturnsAsync(false);
            _userRepoMock.Setup(repo => repo.GetRolebyNameAsync(registrationData.Role)).ReturnsAsync(new Role { Name = registrationData.Role });
            _userRepoMock.Setup(repo => repo.RegisterAsync(It.IsAny<WMSuser>())).ReturnsAsync(true);

            // Act
            var result = await _userController.Register(registrationData);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequestWithNonExistingRole()
        {
            // Arrange
            var registrationData = new RegistrationRequest
            {
                Username = "newUsername",
                Password = "newPassword",
                Name = "New User",
                Role = "NonExistingRole"
            };
            var newUser = new WMSuser { Username = registrationData.Username, Id = 1, Role = new Role { Name = registrationData.Role }, Active = true };

            _userRepoMock.Setup(repo => repo.IsUniqueUserAsync(registrationData.Username)).ReturnsAsync(true);
            _userRepoMock.Setup(repo => repo.GetRolebyNameAsync(registrationData.Role)).ReturnsAsync(null as Role);
            _userRepoMock.Setup(repo => repo.RegisterAsync(It.IsAny<WMSuser>())).ReturnsAsync(true);

            // Act
            var result = await _userController.Register(registrationData);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequestWithInvalidData()
        {
            // Arrange
            var registrationData = new RegistrationRequest
            {
                Username = "newUsername",
                Password = "newPassword",
                Name = "New User",
                Role = "Customer"
            };
            var newUser = new WMSuser { Username = registrationData.Username, Id = 1, Role = new Role { Name = registrationData.Role }, Active = true };

            _userRepoMock.Setup(repo => repo.IsUniqueUserAsync(registrationData.Username)).ReturnsAsync(true);
            _userRepoMock.Setup(repo => repo.GetRolebyNameAsync(registrationData.Role)).ReturnsAsync(new Role { Name = registrationData.Role });
            _userRepoMock.Setup(repo => repo.RegisterAsync(It.IsAny<WMSuser>())).ReturnsAsync(false);

            // Act
            var result = await _userController.Register(registrationData);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Login_ReturnsInternalServerError()
        {
            // Arrange
            var username = "validUsername";
            var password = "validPassword";
            var token = "validToken";
            var loginUser = new WMSuser { Username = username, Role = new Role { Name = "Customer" }, Active = true };

            _jwtServiceMock.Setup(jwt => jwt.GetJwtToken(loginUser.Id, loginUser.Role.Name)).Returns(token);
            _userRepoMock.Setup(repo => repo.LoginAsync(username, password)).ThrowsAsync(new Exception());

            var loginData = new LoginRequest { Username = username, Password = password };

            // Act
            var result = await _userController.Login(loginData);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnsInternalServerError()
        {
            // Arrange
            var registrationData = new RegistrationRequest
            {
                Username = "newUsername",
                Password = "newPassword",
                Name = "New User",
                Role = "Customer"
            };
            var newUser = new WMSuser { Username = registrationData.Username, Id = 1, Role = new Role { Name = registrationData.Role }, Active = true };

            _userRepoMock.Setup(repo => repo.IsUniqueUserAsync(registrationData.Username)).ThrowsAsync(new Exception());
            _userRepoMock.Setup(repo => repo.GetRolebyNameAsync(registrationData.Role)).ReturnsAsync(new Role { Name = registrationData.Role });
            _userRepoMock.Setup(repo => repo.RegisterAsync(It.IsAny<WMSuser>())).ReturnsAsync(true);

            // Act
            var result = await _userController.Register(registrationData);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}