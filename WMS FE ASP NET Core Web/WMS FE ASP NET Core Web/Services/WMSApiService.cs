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
        public WMSApiService(ApiClient apiClient, ILogger<WMSApiService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var login = new LoginRequestModel { Username = username, Password = password };
            var response = await _apiClient.PostAsync("login", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(await response.Content.ReadAsStringAsync());
                    var loginResponse = await _apiClient.GetDeserializeContent<LoginResponseModel>(response.Content);
                    _apiClient.SetBearerToken(loginResponse.Token);
                    _logger.LogInformation($"Login {username} succeeded.");
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Login {username} failed. Exception Error: {e.Message}");
                    return false;                    
                }

            }
            else
            {
                _logger.LogError($"Login {username} failed.");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _apiClient.PostAsync("logout", null);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Logout succeeded.");
                return true;
            }
            else
            {
                _logger.LogError($"Logout failed.");
                return false;
            }
        }
        /*
        public async Task<List<InventoryItem>> GetInventoryItemsAsync()
        {
            var response = await _apiClient.GetAsync("inventory");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var inventoryItems = JsonConvert.DeserializeObject<List<InventoryItem>>(await response.Content.ReadAsStringAsync());
                    var inventoryItems = await _apiClient.GetDeserializeContent<List<InventoryItem>>(response.Content);
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

        public async Task<InventoryItem> GetInventoryItemAsync(int id)
        {
            var response = await _apiClient.GetAsync($"inventory/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var inventoryItem = JsonConvert.DeserializeObject<InventoryItem>(await response.Content.ReadAsStringAsync());
                    var inventoryItem = await _apiClient.GetDeserializeContent<InventoryItem>(response.Content);
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

        public async Task<bool> UpdateInventoryItemAsync(InventoryItem inventoryItem)
        {
            var response = await _apiClient.PutAsync($"inventory/{inventoryItem.Id}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));
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
            var response = await _apiClient.DeleteAsync($"inventory/{id}");
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
        public async Task<bool> CreateInventoryItemAsync(InventoryItem inventoryItem)
        {
            var response = await _apiClient.PostAsync($"inventory", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));
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
        */
        public async Task<List<GetCustomerModel>> GetCustomersAsync()
        {
            var response = await _apiClient.GetAsync("/GetCustomers");
            if (response.IsSuccessStatusCode)
            {
                try
                {                    
                    var customers = await _apiClient.GetDeserializeContent<List<GetCustomerModel>>(response.Content);
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
    }
}
