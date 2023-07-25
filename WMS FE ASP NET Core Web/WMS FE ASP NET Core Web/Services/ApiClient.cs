using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private string _bearerToken;

        public ApiClient(string baseAddress, ILogger<ApiClient> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));           
            // Add an Accept header for JSON format.
            _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("*/*"));
            _logger = logger;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"GET {url} returned {response.StatusCode}");
            }
            else
            {
                _logger.LogError($"GET {url} returned {response.StatusCode}");
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"POST {url} returned {response.StatusCode}");
            }
            else
            {
                _logger.LogError($"POST {url} returned {response.StatusCode}");
            }
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            var response = await _httpClient.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"PUT {url} returned {response.StatusCode}");
            }
            else
            {
                _logger.LogError($"PUT {url} returned {response.StatusCode}");
            }
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"DELETE {url} returned {response.StatusCode}");
            }
            else
            {
                _logger.LogError($"DELETE {url} returned {response.StatusCode}");
            }
            return response;
        }

        internal void SetBearerToken(object? token)
        {
            _bearerToken = token?.ToString() ?? "";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        internal async Task<T> GetDeserializeContent<T>(HttpContent content)
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
                    T resultContent = serializer.Deserialize<T>(reader);
                    return resultContent;
                }
            }

        }
    }
}
