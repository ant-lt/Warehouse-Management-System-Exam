using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
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
        public string userName { get; private set; } = string.Empty;
        public string role { get; private set; } = string.Empty;
        public int userId { get; private set; }
        public string token { get; private set; } = string.Empty;
        public string errorMessage { get; private set; } = string.Empty;

        public WMSApiService(ApiClient apiClient, ILogger<WMSApiService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Logs in the user with the given username and password.
        /// </summary>
        /// <param name="username">The username of the user to be logged in.</param>
        /// <param name="password">The password of the user to be logged in.</param>
        /// <returns>True if the login is successful and false otherwise.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
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
                    
                    if (loginResponse != null) 
                    { 
                        userName = loginResponse.UserName;
                        role = loginResponse.Role;
                        userId = loginResponse.UserId;
                        token = loginResponse.Token;
                        _logger.LogInformation($"Login {username} in role {role} succeeded.");
                        return true;
                    }
                    else
                    {
                        errorMessage = "Login failed. Login response is null.";
                        _logger.LogError($"Login {username} failed. Login response is null.");
                        return false;
                    }
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
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

        /// <summary>
        /// Set API parameters using a collection of claims.
        /// </summary>
        /// <param name="claims">Collection of claims containing WMSUserId and WMS API Token values.</param>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
        public void SetAPIParams(IEnumerable<Claim> claims)
        {
            try
            {
                errorMessage = string.Empty;

                var getWMSUserIdClaim = claims.FirstOrDefault(c => c.Type == "WMSUserId");
                if (getWMSUserIdClaim != null)
                {
                    userId = int.Parse(getWMSUserIdClaim.Value);
                }

                var getAPITokenClaim = claims.FirstOrDefault(c => c.Type == "APIToken");
                if (getAPITokenClaim != null)
                {
                    token = getAPITokenClaim.Value;
                    _apiClient.SetBearerToken(token);
                }
                _logger.LogInformation($"SetAPIParams() for WMSUserId = {userId} succeeded.");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"SetAPIParams() failed. Exception Error: {e.Message}");
            }
        }

        /// <summary>
        /// Checks if the JWT token has expired.
        /// </summary>
        /// <returns>Returns true if the token has expired, otherwise returns false.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
        public bool IsTokenExpired()
        {
            try
            {
                // Decode the JWT token's payload (second part)
                string[] tokenParts = token.Split('.');
                string padding = new string('=', (4 - tokenParts[1].Length % 4) % 4); // Add padding
                string payload = tokenParts[1].Replace("_", "/");

                byte[] payloadBytes = Convert.FromBase64String(payload + padding);
                string payloadJson = Encoding.UTF8.GetString(payloadBytes);

                // Parse the payload JSON to a dictionary
                var payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);

                // Extract the 'exp' claim (expiration timestamp) from the payload
                if (payloadData != null && payloadData.TryGetValue("exp", out var expClaimValue) && expClaimValue is long expUnixTimestamp)
                {
                    // Convert the UNIX timestamp to DateTime
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTimestamp).UtcDateTime;

                    // Check if the token has expired
                    return expirationTime <= DateTime.UtcNow;
                }

                // 'exp' claim not found or invalid, consider token as expired               
                _logger.LogError("The JWT token has expired.");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"IsTokenExpired() failed. Exception Error: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// The PostWMSDataAsync method sends a POST request to a specified URL with the data provided as input. 
        /// It is a generic asynchronous method that can take in two type parameters: TT and T. 
        /// The TT type parameter is the response type and must be a class. 
        /// The T type parameter refers to the data type sent in the request and can be any type.
        /// </summary>
        /// <typeparam name="TT">Response type TT</typeparam>
        /// <typeparam name="T">Input data type T</typeparam>
        /// <param name="data">The data to be sent with the request</param>
        /// <param name="url">The URL to send the request to</param>
        /// <returns>The deserialized response as type TT</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>        
        public async Task<TT?> PostWMSDataAsync<TT, T>(T data, string url) where TT : class
        {
            try
            {
                errorMessage = string.Empty;
                var response = await _apiClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"PostWMSDataAsync<{typeof(TT).Name},{typeof(T).Name}>({url}) succeeded. Response code: {response.StatusCode}");
                    return await _apiClient.GetDeserializeContent<TT>(response.Content);
                }
                else
                {
                    var errorResponse = await _apiClient.GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse?.Message ?? $"Error on PostWMSDataAsync<{typeof(TT).Name},{typeof(T).Name}>({url})";
                    _logger.LogError($"PostWMSDataAsync<{typeof(TT).Name},{typeof(T).Name}>({url}) failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"PostWMSDataAsync<{typeof(T).Name}>({url}) failed. Exception Error: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of WMS data asynchronously from a specified URL.
        /// </summary>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <param name="url">The URL to retrieve the data from.</param>
        /// <returns>A List of retrieved WMS data.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>        
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
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
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

        /// <summary>
        /// A generic HTTP GET method which sends a request to the specified URL and 
        /// retrieves the response body deserialized as an object of type T.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized response object.</typeparam>
        /// <param name="url">The URL where the HTTP GET request is sent.</param>
        /// <returns>The deserialized object of type T as the HTTP GET response body.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
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
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
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

        /// <summary>
        /// Updates data in WMS API asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data to be updated.</typeparam>
        /// <param name="data">The data to be updated.</param>
        /// <param name="url">The URL of the API endpoint to update the data.</param>
        /// <returns>A boolean to indicate if the update was successful or not.</returns>
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
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
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

        /// <summary>
        /// Deletes data in the WMS system via the specified URL.
        /// </summary>
        /// <param name="url">The URL to delete data from.</param>
        /// <returns>A Task representing the result of the asynchronous operation. Returns true if the delete operation was successful, or false otherwise.</returns>
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
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
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
