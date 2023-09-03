using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;
using Microsoft.AspNetCore.Cors;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    [EnableCors("WMSCorsPolicy")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly WMSApiService _wmsApiService;

        public LoginController(ILogger<LoginController> logger, WMSApiService wmsApiService)
        {
            _logger = logger;
            _wmsApiService = wmsApiService;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _wmsApiService.LoginAsync(loginModel.UserName, loginModel.Password);
                if (result)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _wmsApiService.userName),
                        new Claim(ClaimTypes.Role, _wmsApiService.role),
                        new Claim("WMSUserId", _wmsApiService.userId.ToString()),
                        new Claim("APIToken", _wmsApiService.token),
                    };
                    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(principal, authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error: " + _wmsApiService.errorMessage + " Please try again.");
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
