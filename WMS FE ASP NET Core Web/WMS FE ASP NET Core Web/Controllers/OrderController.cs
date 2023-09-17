using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WMS_FE_ASP_NET_Core_Web.DTO;
using WMS_FE_ASP_NET_Core_Web.Models;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web.Controllers
{
    [EnableCors("WMSCorsPolicy")]
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WMSApiService _wmsApiService;
        private readonly Iwrapper _wrapper;

        public OrderController(ILogger<HomeController> logger, WMSApiService wMSApiService, Iwrapper wrapper)
        {
            _logger = logger;
            _wmsApiService = wMSApiService;
            _wrapper = wrapper;
        }

        // GET: OrderController/Details/5
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Details(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired()) 
                return RedirectToAction("Logout", "Home");
            
            var order = await _wmsApiService.GetWMSDataAsync<OrderModel>($"/GetOrderBy/{id}");
            var orderItems = await _wmsApiService.GetWMSDataListAsync<OrderItemModel>($"/GetOrderBy/{id}/Items");

            var orderViewModel = _wrapper.Bind(order, orderItems);
            return View(orderViewModel);
        }        

        // GET: OrderController/Create
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create()
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
            
           
            var orderTypes = await _wmsApiService.GetWMSDataListAsync<OrderType>("/GetOrderTypes");
            var customers = await _wmsApiService.GetWMSDataListAsync<CustomerModel>("/GetCustomers");
           
            var order = _wrapper.Bind(orderTypes, customers);
            return View(order);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
        
            var order = _wrapper.BindToCreateOrder(collection, _wmsApiService.userId);
                
            var newOrder = await _wmsApiService.PostWMSDataAsync<CreateNewResourceResponse, CreateOrderModel>(order, $"/CreateNewOrder");

            if (newOrder is not null)
            {               
                return RedirectToAction("Edit", "Order",new { newOrder.Id });
            }
            else
            {
               ViewData["ErrorMessage"] = "Could not create order. Please try again.";
               return View();
            }
        }

        // GET: OrderController/Edit/5
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
            
            var order = await _wmsApiService.GetWMSDataAsync<OrderModel>($"/GetOrderBy/{id}");
            var orderItems = await _wmsApiService.GetWMSDataListAsync<OrderItemModel>($"/GetOrderBy/{id}/Items");
            var products = await _wmsApiService.GetWMSDataListAsync<ProductModel>("/GetProducts");

            var orderViewModel = _wrapper.Bind(order, orderItems, products);

            return View(orderViewModel);
        }


        // GET: Order/Delete/5
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");

            var deleteOrder = await _wmsApiService.DeleteWMSDataAsync($"/Delete/Order/{id}");
            if (deleteOrder)
            {
                return RedirectToAction("Orders", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Could not delete order Id={id}. Please try again.";
                return View();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddOrderItem(int orderId, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired())
                    return RedirectToAction("Logout", "Home");

                var orderItem = _wrapper.Bind(orderId, collection);
                
                var newOrderItem = await _wmsApiService.PostWMSDataAsync<CreateNewResourceResponse, CreateOrderItemModel>(orderItem, $"/CreateOrderItem");
                if (newOrderItem is not null)
                {
                    return RedirectToAction("Edit", "Order", new { id = orderId });
                }
                else
                {
                    ViewData["ErrorMessage"] = $"Could not add order item. Please try again.";
                    return View();
                }
            }
            else
            {
                ViewData["ErrorMessage"] = $"Could not add order item. Please try again.";
                return View();
            }            
        }

        [HttpGet]        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int orderItemId)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");

            var deleteOrderItem = await _wmsApiService.DeleteWMSDataAsync($"/Delete/OrderItem/{orderItemId}");
            if (deleteOrderItem)
            {
                return RedirectToAction("Edit", "Order", new { id = orderId });
            }
            else
            {
                ViewData["ErrorMessage"] = $"Could not delete order item Id={orderItemId}. Please try again.";
                return View();
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        // get order item by id for edit
        public async Task<IActionResult> EditOrderItem(int id, int orderId)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");

            var orderItem = await _wmsApiService.GetWMSDataAsync<OrderItemModel>($"/GetOrderItemBy/{id}");
            if (orderItem is null)
            {
                ViewData["ErrorMessage"] = $"Could not find order item Id={id}. Please try again.";
                return View();
            }
            OrderItemViewModel orderOrderItemView = _wrapper.Bind(orderId, orderItem);

            return View(orderOrderItemView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditOrderItem(int id, int orderId, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                _wmsApiService.SetAPIParams(User.Claims);
                if (_wmsApiService.IsTokenExpired())
                    return RedirectToAction("Logout", "Home");

                var orderItem = _wrapper.BindToUpdateOrderItem(collection);
                var updatedOrderItem = await _wmsApiService.UpdateWMSDataAsync<UpdateOrderItemModel>(orderItem, $"/Update/OrderItem/{id}");
                                
                if (updatedOrderItem)
                {
                    return RedirectToAction("Edit", "Order", new { id = orderId });
                }
                else
                {
                    ViewData["ErrorMessage"] = $"Could not update order item. Please try again.";
                
                    return View();
                }
            }
            else
            {
                ViewData["ErrorMessage"] = $"Could not update order item. Please try again.";
                return View();
            }
        }
    }
}
