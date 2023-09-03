using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_FE_ASP_NET_Core_Web.DTO;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    [EnableCors("WMSCorsPolicy")]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly WMSApiService _wmsApiService;
        private readonly Iwrapper _wrapper;

        public RegisterController(ILogger<RegisterController> logger, WMSApiService wmsApiService, Iwrapper wrapper)
        {
            _logger = logger;
            _wmsApiService = wmsApiService;
            _wrapper = wrapper;
        }

        [HttpGet("Register")]
        [AllowAnonymous]
        // GET: RegisterController
        public ActionResult Register()
        {
            return View();
        }


        // POST: RegisterController
        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormCollection collection)
        {
            var user = _wrapper.BindToRegistrationRequest(collection);               

            // var newCustomer = await _wmsApiService.RegisterNewUser(user);
            var newCustomer = await _wmsApiService.PostWMSDataAsync<RegistrationRequestModel>(user, "/Register");
            if (newCustomer)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Could not register new user. Error: "+ _wmsApiService.errorMessage+ " Please try again.");                   
                return View();
            }                
        }
    }
}
