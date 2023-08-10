using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WMSApiService _wmsApiService;

        public OrderController(ILogger<HomeController> logger, WMSApiService wMSApiService)
        {
            _logger = logger;
            _wmsApiService = wMSApiService;
        }

        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: OrderController/Edit/5
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _wmsApiService.GetOrderAsync(id);
            var orderItems = await _wmsApiService.GetOrderItemsAsync(id);

            var orderViewModel = new OrderViewModel
            {

                Order = order ?? new OrderModel(),
                OrderItems = orderItems ?? new List<OrderItemModel>()
            };

            return View(orderViewModel);
        }


        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
