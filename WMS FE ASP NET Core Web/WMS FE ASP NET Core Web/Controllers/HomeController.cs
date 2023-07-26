using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;

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
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

       
        public async Task<IActionResult> Customers()
        {
            var customers = await _wmsApiService.GetCustomersAsync();
            return View(customers);
        }

       
        public async Task<IActionResult> Inventory()
        {
            var inventory = await _wmsApiService.GetInventoryItemsAsync();
            return View(inventory);
        }

        
        public async Task<IActionResult> Orders()
        {
            var orders = await _wmsApiService.GetOrdersAsync();
            return View(orders);
        }

       
        public async Task<IActionResult> Products()
        {
            var products = await _wmsApiService.GetProductsAsync();
            return View(products);
        }

      
        public IActionResult Reports()
        {
            return View();
        }

        /// <summary>
        /// Logout action
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}