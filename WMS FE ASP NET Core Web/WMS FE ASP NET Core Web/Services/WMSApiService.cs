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
        
        public async Task<bool> PostWMSDataAsync<T>(T data, string url)
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"PostWMSDataAsync<{typeof(T).Name}>({url}) succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"PostWMSDataAsync<{typeof(T).Name}>({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return false;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"PostWMSDataAsync<{typeof(T).Name}>({url}) failed. Exception Error: {e.Message}");
                return false;
            }
        }

        public async Task<List<T>?> GetWMSDataListAsync<T>(string url)
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    var dataList = await _apiClient.GetDeserializeContent<List<T>>(response.Content);
                    _logger.LogInformation($"GetWMSDataListAsync<{typeof(T).Name}>({url}) succeeded.");
                    return dataList;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"GetWMSDataListAsync<{typeof(T).Name}>({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"GetWMSDataListAsync<{typeof(T).Name}>({url}) failed. Exception Error: {e.Message}");
                return null;
            }
        }

        public async Task<T?> GetWMSDataAsync<T>(string url) where T : class
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await _apiClient.GetDeserializeContent<T>(response.Content);
                    _logger.LogInformation($"GetWMSDataAsync<{typeof(T).Name}>({url}) succeeded.");
                    return data;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"GetWMSDataAsync<{typeof(T).Name}>({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"GetWMSDataAsync<{typeof(T).Name}>({url}) failed. Exception Error: {e.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateWMSDataAsync<T>(T data, string url)
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"UpdateWMSDataAsync<{typeof(T).Name}>({url}) succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"UpdateWMSDataAsync<{typeof(T).Name}>({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return false;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"UpdateWMSDataAsync<{typeof(T).Name}>({url}) failed. Exception Error: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteWMSDataAsync(string url)
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"DeleteWMSDataAsync({url}) succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse.Message;
                    _logger.LogError($"DeleteWMSDataAsync({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return false;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"DeleteWMSDataAsync({url}) failed. Exception Error: {e.Message}");
                return false;
            }
        }
    }
}
