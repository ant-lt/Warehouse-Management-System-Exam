using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}
