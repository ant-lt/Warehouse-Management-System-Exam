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
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly WMSApiService _wmsApiService;
        private readonly Iwrapper _wrapper;

        public CustomerController(ILogger<CustomerController> logger, WMSApiService wmsApiService, Iwrapper wrapper)
        {
            _logger = logger;
            _wmsApiService = wmsApiService;
            _wrapper = wrapper;
        }

        // GET: Customer/Details/{id}
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Details(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");

            var customer = await _wmsApiService.GetWMSDataAsync<CustomerModel>($"/GetCustomerBy/{id}");
            return View(customer);           
        }

        // GET: Customer/Create
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");
            var customer = _wrapper.Bind(collection);

            var newCustomer = await _wmsApiService.PostWMSDataAsync<CreateNewResourceResponse, CreateCustomerModel>(customer, $"/CreateNewCustomer");
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
        public async Task<IActionResult> Edit(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired()) 
                return RedirectToAction("Logout", "Home");
            var customer = await _wmsApiService.GetWMSDataAsync<CustomerModel>($"/GetCustomerBy/{id}");
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {             
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");

            var updatedCustomer = _wrapper.BindToUpdateCustomer(collection);
            var updated = await _wmsApiService.UpdateWMSDataAsync<UpdateCustomerModel>(updatedCustomer, $"/Update/Customer/{id}");
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
        public async Task<IActionResult> Delete(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");

            var deleted = await _wmsApiService.DeleteWMSDataAsync($"/Delete/Customer/{id}");
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
