using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WMS.Domain.Models;
using WMS.Infastructure.Interfaces;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication and registration.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase

    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userRepo">An instance of the user repository interface responsible for data access.</param>
        /// <param name="logger">An instance of the logger interface for logging controller actions and events.</param>
        /// <param name="jwtService">An instance of the Jwt token service interface.</param>
        /// <param name="passwordService">An instance of the password service interface.</param>
        public UserController(IUserRepository userRepo, ILogger<UserController> logger, IJwtService jwtService, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _logger = logger;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Attempts to log in to the Warehouse Management System (WMS) with the provided login data.
        /// </summary>
        /// <param name="loginData">Login data including the username and password.</param>
        /// <returns>A login response indicating whether the login attempt was successful.</returns>
        /// <response code="200">OK: The login attempt was successful, and the login response is returned.</response>
        /// <response code="400">Bad Request: The request was malformed or contained invalid data.</response>
        /// <response code="401">Unauthorized: The client was not authenticated, or the login credentials are invalid.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpPost("/Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginData)
        {

            _logger.LogInformation($"Login with username: {loginData.Username}");

            try
            {
                var loginUser = await _userRepo.LoginAsync(loginData.Username, loginData.Password);

                if (loginUser == null)
                {
                    return Unauthorized(new { message = "Username or password is incorrect" });
                }

                if (!loginUser.Active)
                {
                    return BadRequest(new { message = "User is inactive. Contact WMS system administrator." });
                }

                var token = _jwtService.GetJwtToken(loginUser.Id, loginUser.Role.Name);

                return Ok(new LoginResponse { UserName = loginData.Username, Active = loginUser.Active, Role = loginUser.Role.Name, Token = token, UserId = loginUser.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} HttpPost Login exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Registers a new user in the Warehouse Management System (WMS) system.
        /// </summary>
        /// <param name="registrationData">Registration data containing user information.</param>
        /// <returns>The HTTP status code indicating the result of the registration attempt.</returns>
        /// <response code="201">Created: The user registration was successful.</response>
        /// <response code="400">Bad Request: The request was malformed or contained invalid data.</response>
        /// <response code="500">Internal Server Error: An internal server error occurred while processing the request.</response>
        [HttpPost("/Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<CreateNewResourceResponseDto>> Register([FromBody] RegistrationRequest registrationData)
        {
            _logger.LogInformation($"Register with username: {registrationData.Username}  role: {registrationData.Role}");

            try
            {
                var isUserNameUnique = await _userRepo.IsUniqueUserAsync(registrationData.Username);

                if (!isUserNameUnique)
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                _passwordService.CreatePasswordHash(registrationData.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var userRole = await _userRepo.GetRolebyNameAsync(registrationData.Role);

                if (userRole == null)
                {
                    return BadRequest(new { message = "Role not exists" });
                }

                WMSuser newUser = new()
                {
                    Username = registrationData.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Name = registrationData.Name ?? string.Empty,
                    Role = userRole,
                    Active = true,
                };

                var user = await _userRepo.RegisterAsync(newUser);

                if (user == false)
                {
                    return BadRequest(new { message = "Error while new user registering" });
                }

                CreateNewResourceResponseDto registerUserResponseDto = new CreateNewResourceResponseDto()
                {
                    Id = newUser.Id
                };               
                return CreatedAtAction("Register", registerUserResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} HttpPost Register exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
