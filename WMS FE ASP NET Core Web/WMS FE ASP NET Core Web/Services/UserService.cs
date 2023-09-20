using System.Security.Claims;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public int? GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var getWMSUserIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "WMSUserId");
                if (getWMSUserIdClaim != null)
                    return int.Parse(getWMSUserIdClaim.Value);
                else
                    return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetUserId() failed. Exception Error: {e.Message}");
                return null;
            }
        }
    }
}
