﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    /// <summary>
    /// Service interface for WMS API
    /// </summary>
    public class WMSApiService
    {
        //readonly ApiClient _apiClient;
        private readonly HttpClient _httpClient;
        readonly ILogger<WMSApiService> _logger;
        public string errorMessage { get; private set; } = string.Empty;

        public WMSApiService(HttpClient client, ILogger<WMSApiService> logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        /// <summary>
        /// Logs in the user with the given username and password.
        /// </summary>
        /// <param name="username">The username of the user to be logged in.</param>
        /// <param name="password">The password of the user to be logged in.</param>
        /// <returns>True if the login is successful and false otherwise.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
        public async Task<LoginResponseModel?> LoginAsync(string username, string password)
        {
            try
            { 
                errorMessage = string.Empty;
                var login = new LoginRequestModel { Username = username, Password = password };
                var response = await _httpClient.PostAsync("login", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {                    
                    var loginResponse = await GetDeserializeContent<LoginResponseModel>(response.Content);
                    
                    if (loginResponse != null) 
                    { 
                        _logger.LogInformation($"Login {loginResponse.UserName} in role {loginResponse.Role} succeeded.");
                        return loginResponse;
                    }
                    else
                    {
                        errorMessage = "Login failed. Login response is null.";
                        _logger.LogError($"Login {username} failed. Login response is null.");
                        return null;
                    }
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
                    errorMessage = errorResponse?.Message ?? "Error response is null.";
                    _logger.LogError($"Login {username} failed. Response status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                _logger.LogError($"Login {username} failed. Exception Error: {e.Message}");
                return null;
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
        /// <param name="apiToken">The API token to be used for authentication</param>
        /// <returns>The deserialized response as type TT</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>        
        public async Task<TT?> PostWMSDataAsync<TT, T>(T? data, string url, string apiToken) where TT : class
        {
            try
            {
                StringContent? content = null;
                HttpResponseMessage? response = null;

                errorMessage = string.Empty;

                SetBearerToken(apiToken);

                if (data != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");                 
                }

                response = await _httpClient.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"PostWMSDataAsync<{typeof(TT).Name},{typeof(T).Name}>({url}) succeeded. Response code: {response.StatusCode}");
                    return await GetDeserializeContent<TT>(response.Content);
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
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
        /// <param name="apiToken">The API token to be used for authentication.</param>
        /// <returns>A List of retrieved WMS data.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>        
        public async Task<List<T>?> GetWMSDataListAsync<T>(string url, string apiToken)
        {
            try
            {
                errorMessage = string.Empty;
                SetBearerToken(apiToken);
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    var dataList = await GetDeserializeContent<List<T>>(response.Content);
                    _logger.LogInformation($"GetWMSDataListAsync<{typeof(T).Name}>({url}) succeeded.");
                    return dataList;
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
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
        /// <param name="apiToken">The API token to be used for authentication.</param>
        /// <returns>The deserialized object of type T as the HTTP GET response body.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
        public async Task<T?> GetWMSDataAsync<T>(string url, string apiToken) where T : class
        {
            try
            {
                errorMessage = string.Empty;
                SetBearerToken(apiToken);
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await GetDeserializeContent<T>(response.Content);
                    _logger.LogInformation($"GetWMSDataAsync<{typeof(T).Name}>({url}) succeeded.");
                    return data;
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
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
        /// <param name="apiToken">The API token to be used for authentication.</param>
        /// <returns>A boolean to indicate if the update was successful or not.</returns>
        public async Task<bool> UpdateWMSDataAsync<T>(T data, string url, string apiToken)
        {
            try
            {
                errorMessage = string.Empty;
                SetBearerToken(apiToken);
                var response = await _httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"UpdateWMSDataAsync<{typeof(T).Name}>({url}) succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
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
        /// <param name="apiToken">The API token to be used for authentication.</param>
        /// <returns>A Task representing the result of the asynchronous operation. Returns true if the delete operation was successful, or false otherwise.</returns>
        public async Task<bool> DeleteWMSDataAsync(string url, string apiToken)
        {
            try
            {
                errorMessage = string.Empty;
                SetBearerToken(apiToken);
                var response = await _httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"DeleteWMSDataAsync({url}) succeeded.");
                    return true;
                }
                else
                {
                    var errorResponse = await GetDeserializeContent<ErrorResponse>(response.Content);
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

        internal void SetBearerToken(object? token)
        {
            string _bearerToken = token?.ToString() ?? "";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        internal async Task<T?> GetDeserializeContent<T>(HttpContent content)
        {
            // Use a JsonSerializerSettings object to configure the JsonSerializer object.
            // This can improve performance by reducing the amount of reflection that is performed by the JsonSerializer.
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            JsonSerializer serializer = JsonSerializer.Create(settings);

            //Use the ConfigureAwait(false) method when awaiting the ReadAsStreamAsync() method.
            //This can improve performance by reducing the amount of context switching that occurs when the method is awaited.
            using (Stream s = await content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                //Use a JsonTextReader object instead of a StreamReader object to read the JSON data. This can improve performance by reducing the amount of memory that is used to store the JSON data. 
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(s)))
                {
                    T? resultContent = serializer.Deserialize<T>(reader);
                    return resultContent;
                }
            }
        }
    }
}
