using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
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

        public UserController(IUserRepository userRepo, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        /// <summary>
        /// Login to WMS system
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns>Status code</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginData)
        {

            _logger.LogInformation($"Login with username: {loginData.Username}");

            try
            {
                var loginResponse = await _userRepo.LoginAsync(loginData);

                if (loginResponse.UserName == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                if (!loginResponse.Active)
                {
                    return BadRequest(new { message = "User is inactive. Contact WMS system administrator." });
                }

                return Ok(loginResponse);
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
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

                var user = await _userRepo.RegisterAsync(registrationData);

                if (user == null)
                {
                    return BadRequest(new { message = "Error while registering" });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} HttpPost Register exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
