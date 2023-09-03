﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
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
        private CreateOrderViewModel _order ;

        public OrderController(ILogger<HomeController> logger, WMSApiService wMSApiService, Iwrapper wrapper)
        {
            _logger = logger;
            _wmsApiService = wMSApiService;
            _order = new CreateOrderViewModel();
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
            
            double totalVolume = 0;
            
            if (orderItems != null )
            {
                foreach (var item in orderItems!)
                {
                    totalVolume += item.Volume;
                }
            }
            
            var orderViewModel = new OrderViewModel
            {
                Order = order ?? new OrderModel(),
                OrderItems = orderItems ?? new List<OrderItemModel>(),
                TotalVolume = totalVolume
            };
            return View(orderViewModel);
        }        

        // GET: OrderController/Create
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create()
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
            
            _order = new CreateOrderViewModel
            {
                OrderTypes = await _wmsApiService.GetWMSDataListAsync<OrderType>("/GetOrderTypes"),
                Customers = await _wmsApiService.GetWMSDataListAsync<CustomerModel>("/GetCustomers"),
            };
            return View(_order);
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
                
            var newOrder = await _wmsApiService.PostWMSDataAsync<CreateOrderModel>(order, $"/CreateNewOrder");

            if (newOrder)
            {
                return RedirectToAction("Orders", "Home");
            }
            else
            {
               ViewData["ErrorMessage"] = "Could not create order. Please try again.";
               return View();
            }
        }
        
        // GET: OrderController/Edit/5
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
            
            var order = await _wmsApiService.GetWMSDataAsync<OrderModel>($"/GetOrderBy/{id}");
            var orderItems = await _wmsApiService.GetWMSDataListAsync<OrderItemModel>($"/GetOrderBy/{id}/Items");

            double totalVolume = 0;
            if (orderItems != null)
            {
                foreach (var item in orderItems!)
                {
                    totalVolume += item.Volume;
                }
            }

            var orderViewModel = new OrderViewModel
            {
                Order = order ?? new OrderModel(),
                OrderItems = orderItems ?? new List<OrderItemModel>(),
                TotalVolume = totalVolume
            };
            return View(orderViewModel);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            _wmsApiService.SetAPIParams(User.Claims);
            if (_wmsApiService.IsTokenExpired())
                return RedirectToAction("Logout", "Home");
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Delete/5
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
    }
}
