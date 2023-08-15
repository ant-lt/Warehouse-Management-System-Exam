using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;
using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    /// <summary>
    /// Service interface for WMS API
    /// </summary>
    public class WMSApiService
    {
        readonly ApiClient _apiClient;
        readonly ILogger<WMSApiService> _logger;
        public string userName { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string errorMessage { get; private set; } = string.Empty;

        public WMSApiService(ApiClient apiClient, ILogger<WMSApiService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            { 
                errorMessage = string.Empty;
                var login = new LoginRequestModel { Username = username, Password = password };
                var response = await _apiClient.PostAsync("login", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {                    
                    var loginResponse = await _apiClient.GetDeserializeContent<LoginResponseModel>(response.Content);
                    _apiClient.SetBearerToken(loginResponse.Token);
                    userName = loginResponse.UserName;
                    role = loginResponse.Role;
                    _logger.LogInformation($"Login {username} succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"Login {username} failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return false;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"Login {username} failed. Exception Error: {e.Message}");
                return false;
            }
        }
        
        public async Task<List<CustomerModel>> GetCustomersAsync()
        {
            var response = await _apiClient.GetAsync("/GetCustomers");
            if (response.IsSuccessStatusCode)
            {
                try
                {                    
                    var customers = await _apiClient.GetDeserializeContent<List<CustomerModel>>(response.Content);
                    _logger.LogInformation($"GetCustomersAsync succeeded.");
                    return customers;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetCustomersAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetCustomersAsync failed.");
                return null;
            }
        }

        public async Task<CustomerModel> GetCustomerAsync(int id)
        {
            var response = await _apiClient.GetAsync($"/GetCustomerBy/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var customer = await _apiClient.GetDeserializeContent<CustomerModel>(response.Content);
                    _logger.LogInformation($"GetCustomerAsync succeeded.");
                    return customer;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetCustomerAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetCustomerAsync failed.");
                return null;
            }
        }

        public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerModel customer)
        {
            var response = await _apiClient.PutAsync($"/Update/Customer/{id}", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UpdateCustomerAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"UpdateCustomerAsync failed.");
                return false;
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var response = await _apiClient.DeleteAsync($"/Delete/Customer/{id}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DeleteCustomerAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"DeleteCustomerAsync failed.");
                return false;
            }
        }

        public async Task<bool> CreateCustomerAsync(CreateCustomerModel customer)
        {
            var response = await _apiClient.PostAsync($"/CreateNewCustomer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"CreateCustomerAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"CreateCustomerAsync failed.");
                return false;
            }
        }

        public async Task<List<ProductModel>?> GetProductsAsync()
        {
            var response = await _apiClient.GetAsync("/GetProducts");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var products = await _apiClient.GetDeserializeContent<List<ProductModel>>(response.Content);
                    _logger.LogInformation($"GetProductsAsync succeeded.");
                    return products;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetProductsAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetProductsAsync failed.");
                return null;
            }
        }

        public async Task<ProductModel?> GetProductAsync(int id)
        {
            var response = await _apiClient.GetAsync($"/GetProduct/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var product = await _apiClient.GetDeserializeContent<ProductModel>(response.Content);
                    _logger.LogInformation($"GetProductAsync succeeded.");
                    return product;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetProductAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetProductAsync failed.");
                return null;
            }
        }

        public async Task<bool> UpdateProductAsync(ProductModel product)
        {
            var response = await _apiClient.PutAsync($"/UpdateProduct/{product.Id}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UpdateProductAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"UpdateProductAsync failed.");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _apiClient.DeleteAsync($"/DeleteProduct/{id}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DeleteProductAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"DeleteProductAsync failed.");
                return false;
            }
        }

        public async Task<bool> CreateProductAsync(ProductModel product)
        {
            var response = await _apiClient.PostAsync($"/CreateProduct", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"CreateProductAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"CreateProductAsync failed.");
                return false;
            }
        }

        public async Task<List<InventoryItemModel>?> GetInventoryItemsAsync()
        {
            var response = await _apiClient.GetAsync("/GetInventories");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var inventoryItems = await _apiClient.GetDeserializeContent<List<InventoryItemModel>>(response.Content);
                    _logger.LogInformation($"GetInventoryItemsAsync succeeded.");
                    return inventoryItems;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetInventoryItemsAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetInventoryItemsAsync failed.");
                return null;
            }
        }

        public async Task<InventoryItemModel?> GetInventoryItemAsync(int id)
        {
            var response = await _apiClient.GetAsync($"/GetInventoryItem/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var inventoryItem = await _apiClient.GetDeserializeContent<InventoryItemModel>(response.Content);
                    _logger.LogInformation($"GetInventoryItemAsync succeeded.");
                    return inventoryItem;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetInventoryItemAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetInventoryItemAsync failed.");
                return null;
            }
        }

        public async Task<bool> UpdateInventoryItemAsync(InventoryItemModel inventoryItem)
        {
            var response = await _apiClient.PutAsync($"/UpdateInventoryItem/{inventoryItem.Id}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UpdateInventoryItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"UpdateInventoryItemAsync failed.");
                return false;
            }
        }

        public async Task<bool> DeleteInventoryItemAsync(int id)
        {
            var response = await _apiClient.DeleteAsync($"/DeleteInventoryItem/{id}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DeleteInventoryItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"DeleteInventoryItemAsync failed.");
                return false;
            }
        }

        public async Task<bool> CreateInventoryItemAsync(InventoryItemModel inventoryItem)
        {
            var response = await _apiClient.PostAsync($"/CreateInventoryItem", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"CreateInventoryItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"CreateInventoryItemAsync failed.");
                return false;
            }
        }

        public async Task<List<OrderModel>?> GetOrdersAsync()
        {
            var response = await _apiClient.GetAsync("/GetOrders");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var orders = await _apiClient.GetDeserializeContent<List<OrderModel>>(response.Content);
                    _logger.LogInformation($"GetOrdersAsync succeeded.");
                    return orders;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetOrdersAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetOrdersAsync failed.");
                return null;
            }
        }
        public async Task<OrderModel?> GetOrderAsync(int id)
        {
            var response = await _apiClient.GetAsync($"/GetOrderBy/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var order = await _apiClient.GetDeserializeContent<OrderModel>(response.Content);
                    _logger.LogInformation($"GetOrderAsync succeeded.");
                    return order;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetOrderAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetOrderAsync failed.");
                return null;
            }
        }
        public async Task<bool> UpdateOrderAsync(OrderModel order)
        {
            var response = await _apiClient.PutAsync($"/UpdateOrder/{order.Id}", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UpdateOrderAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"UpdateOrderAsync failed.");
                return false;
            }
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var response = await _apiClient.DeleteAsync($"/DeleteOrder/{id}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DeleteOrderAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"DeleteOrderAsync failed.");
                return false;
            }
        }
        public async Task<bool> CreateOrderAsync(OrderModel order)
        {
            var response = await _apiClient.PostAsync($"/CreateOrder", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"CreateOrderAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"CreateOrderAsync failed.");
                return false;
            }
        }
        public async Task<List<OrderItemModel>?> GetOrderItemsAsync(int orderId)
        {
            var response = await _apiClient.GetAsync($"/GetOrderBy/{orderId}/Items");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var orderItems = await _apiClient.GetDeserializeContent<List<OrderItemModel>>(response.Content);
                    _logger.LogInformation($"GetOrderItemsAsync succeeded.");
                    return orderItems;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetOrderItemsAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetOrderItemsAsync failed. Response status code: {response.StatusCode}");
                return null;
            }
        }
        public async Task<OrderItemModel?> GetOrderItemAsync(int id)
        {
            var response = await _apiClient.GetAsync($"/GetOrderItem/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var orderItem = await _apiClient.GetDeserializeContent<OrderItemModel>(response.Content);
                    _logger.LogInformation($"GetOrderItemAsync succeeded.");
                    return orderItem;
                }
                catch (Exception e)
                {
                    _logger.LogError($"GetOrderItemAsync failed. Exception Error: {e.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"GetOrderItemAsync failed.");
                return null;
            }
        }
        public async Task<bool> UpdateOrderItemAsync(OrderItemModel orderItem)
        {
            var response = await _apiClient.PutAsync($"/UpdateOrderItem/{orderItem.Id}", new StringContent(JsonConvert.SerializeObject(orderItem), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UpdateOrderItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"UpdateOrderItemAsync failed.");
                return false;
            }
        }
        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var response = await _apiClient.DeleteAsync($"/DeleteOrderItem/{id}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DeleteOrderItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"DeleteOrderItemAsync failed.");
                return false;
            }
        }
        public async Task<bool> CreateOrderItemAsync(OrderItemModel orderItem)
        {
            var response = await _apiClient.PostAsync($"/CreateOrderItem", new StringContent(JsonConvert.SerializeObject(orderItem), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"CreateOrderItemAsync succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"CreateOrderItemAsync failed.");
                return false;
            }
        }

        public async Task<List<WarehousesRatioOfOccupiedModel>?> GetWareusesRatioOfOccupiedReportsAsync()
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.GetAsync("/GetWarehousesRatioOfOccupied");
                if (response.IsSuccessStatusCode)
                {

                    var warehousesRatioOfOccupied = await _apiClient.GetDeserializeContent<List<WarehousesRatioOfOccupiedModel>>(response.Content);
                    _logger.LogInformation($"GetWareusesRatioOfOccupiedReportsAsync succeeded.");
                    return warehousesRatioOfOccupied;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"GetWareusesRatioOfOccupiedReportsAsync failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"GetWareusesRatioOfOccupiedReportsAsync failed. Exception Error: {e.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterNewUser (RegistrationRequestModel newUser)
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.PostAsync($"/Register", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"RegisterNewUser {newUser.Username} succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"RegisterNewUser failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return false;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"RegisterNewUser failed. Exception Error: {e.Message}");
                return false;
            }
        }
    }
}
