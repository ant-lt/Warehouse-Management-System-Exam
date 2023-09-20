using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using WMS_FE_ASP_NET_Core_Web.DTO;
using Microsoft.AspNetCore.Cors;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    [EnableCors("WMSCorsPolicy")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WMSApiService _wmsApiService;
        private readonly TokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, WMSApiService wMSApiService, TokenService tokenService)
        {
            _logger = logger;
            _wmsApiService = wMSApiService;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
          
        }

        public IActionResult Register()
        {
            return View();
        }

      
        public IActionResult Login()
        {
            return RedirectToAction("Login","Login");            
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Customers()
        {
            string apiToken = _tokenService.GetAPIToken(User);

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");
            
            var customers = await _wmsApiService.GetWMSDataListAsync<CustomerModel>("/GetCustomers", apiToken);
            return View(customers);
        }        

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Inventory()
        {
            string apiToken = _tokenService.GetAPIToken(User);

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");
            var inventory = await _wmsApiService.GetWMSDataListAsync<InventoryItemModel>("/GetInventories", apiToken);
            return View(inventory);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Orders()
        {  
            string apiToken = _tokenService.GetAPIToken(User);

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var orders = await _wmsApiService.GetWMSDataListAsync<OrderModel>("/GetOrders", apiToken);
            return View(orders);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Products()
        {
            string apiToken = _tokenService.GetAPIToken(User);

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var products = await _wmsApiService.GetWMSDataListAsync<ProductModel>("/GetProducts", apiToken);
            return View(products);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Reports()
        {
            string apiToken = _tokenService.GetAPIToken(User);

            if (_tokenService.IsTokenExpired(apiToken)) return RedirectToAction("Logout", "Home");

            var reports = await _wmsApiService.GetWMSDataListAsync<WarehousesRatioOfOccupiedModel>("/GetWarehousesRatioOfOccupied", apiToken);
            return View(reports);
        }

        /// <summary>
        /// Logout action
        /// </summary>
        /// <returns></returns>      
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
