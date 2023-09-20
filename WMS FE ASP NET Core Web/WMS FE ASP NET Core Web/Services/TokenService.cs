using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public class TokenService
    {
        private readonly ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }
        

        /// <summary>
        /// Retrieves the API token from the provided claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal object containing the API token.</param>
        /// <returns>Returns the API token if it exists, otherwise returns an empty string.</returns>
        public string GetAPIToken(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var token = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "APIToken");
                if (token != null)
                    return token.Value;
                else
                    return string.Empty;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAPIToken() failed. Exception Error: {e.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks if the JWT token has expired.
        /// </summary>
        /// <param name="token">The JWT token to check.</param>
        /// <returns>Returns true if the token has expired, otherwise returns false.</returns>
        /// <exception cref="System.Exception">Thrown when an error occurs during the operation.</exception>
        public bool IsTokenExpired(string token)
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
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"IsTokenExpired() failed. Exception Error: {e.Message}");
                return false;
            }
        }
    }
}
