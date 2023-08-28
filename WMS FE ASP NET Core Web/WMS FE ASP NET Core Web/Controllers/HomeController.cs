using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WMSApiService _wmsApiService;

        public HomeController(ILogger<HomeController> logger, WMSApiService wMSApiService)
        {
            _logger = logger;
            _wmsApiService = wMSApiService;
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
            try
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");
            
                var customers = await _wmsApiService.GetWMSDataListAsync<CustomerModel>("/GetCustomers");
                return View(customers);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Inventory()
        {
            try
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");
                var inventory = await _wmsApiService.GetWMSDataListAsync<InventoryItemModel>("/GetInventories");
                return View(inventory);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Orders()
        {  
            try
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");

                var orders = await _wmsApiService.GetWMSDataListAsync<OrderModel>("/GetOrders");
                return View(orders);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Products()
        {
            try
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");

                var products = await _wmsApiService.GetWMSDataListAsync<ProductModel>("/GetProducts");
                return View(products);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Reports()
        {
            try
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired()) return RedirectToAction("Logout", "Home");

                var reports = await _wmsApiService.GetWMSDataListAsync<WarehousesRatioOfOccupiedModel>("/GetWarehousesRatioOfOccupied");
                return View(reports);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Login", "Login");
            }
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
