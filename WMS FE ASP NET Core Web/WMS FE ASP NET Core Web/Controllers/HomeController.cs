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

        public IActionResult Customers()
        {
            var customers = _wmsApiService.GetCustomersAsync().Result;
            return View();
        }

        public IActionResult Inventory()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }

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