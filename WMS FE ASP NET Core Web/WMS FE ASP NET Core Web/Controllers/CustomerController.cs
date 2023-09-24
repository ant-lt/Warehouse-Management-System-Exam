using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_FE_ASP_NET_Core_Web.DTO;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    [EnableCors("WMSCorsPolicy")]
    [Authorize(Roles = "Administrator, Supervisor, Manager")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly WMSApiService _wmsApiService;
        private readonly Iwrapper _wrapper;
        private readonly TokenService _tokenService;
        private readonly ClaimService _claimService;

        public CustomerController(ILogger<CustomerController> logger, WMSApiService wmsApiService, Iwrapper wrapper, TokenService tokenService, ClaimService claimService)
        {
            _logger = logger;
            _wmsApiService = wmsApiService;
            _wrapper = wrapper;
            _tokenService = tokenService;
            _claimService = claimService;
        }

        // GET: Customer/Details/{id}
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public async Task<IActionResult> Details(int id)
        {
            string apiToken = _claimService.GetClaimValue( User, "APIToken");
            
            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var customer = await _wmsApiService.GetWMSDataAsync<CustomerModel>($"/GetCustomerBy/{id}", apiToken);
            return View(customer);
        }

        // GET: Customer/Create
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            string apiToken = _claimService.GetClaimValue(User, "APIToken");

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");
            var customer = _wrapper.Bind(collection);

            var newCustomer = await _wmsApiService.PostWMSDataAsync<CreateNewResourceResponse, CreateCustomerModel>(customer, $"/CreateNewCustomer", apiToken);
            if (newCustomer is not null)
            {
                return RedirectToAction("Customers", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "Could not create customer. Please try again.";
                return View();
            }
        }        

        // GET: Customer/Edit/5
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            string apiToken = _claimService.GetClaimValue(User, "APIToken");
       
            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var customer = await _wmsApiService.GetWMSDataAsync<CustomerModel>($"/GetCustomerBy/{id}", apiToken);
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            string apiToken = _claimService.GetClaimValue(User, "APIToken");

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var updatedCustomer = _wrapper.BindToUpdateCustomer(collection);
            var updated = await _wmsApiService.UpdateWMSDataAsync<UpdateCustomerModel>(updatedCustomer, $"/Update/Customer/{id}", apiToken);
            if (updated)
            {
                return RedirectToAction("Customers", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "Could not update customer. Please try again.";
                return View();
            }               
        }

        // Delete: Customer/Delete/5
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Administrator, Supervisor, Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            string apiToken = _claimService.GetClaimValue(User, "APIToken");

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var deleted = await _wmsApiService.DeleteWMSDataAsync($"/Delete/Customer/{id}", apiToken);
            if (deleted)
            {
                return RedirectToAction("Customers", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "Could not delete customer. Please try again.";
                return View();
            }
        }        
    }
}
