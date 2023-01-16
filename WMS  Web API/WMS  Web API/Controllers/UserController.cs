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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public UserController(IUserRepository userRepo, ILogger<UserController> logger, IJwtService jwtService, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _logger = logger;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Login to WMS system
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns>Status code</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginData)
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
        /// New user registration to WMS system
        /// </summary>
        /// <param name="registrationData"></param>
        /// <returns>Status code</returns>
        /// <response code="201">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("/Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationData)
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
                    Name = registrationData.Name,
                    Role = userRole,
                    Active = true,
                };

                var user = await _userRepo.RegisterAsync(newUser);

                if (user == null)
                {
                    return BadRequest(new { message = "Error while new user registering" });
                }

                return Created(nameof(Login), newUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} HttpPost Register exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
