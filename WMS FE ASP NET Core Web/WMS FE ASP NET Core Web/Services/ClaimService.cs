using System.Security.Claims;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public class ClaimService
    {
        private readonly ILogger<ClaimService> _logger;

        public ClaimService(ILogger<ClaimService> logger)
        {
            _logger = logger;
        }

        public string GetClaimValue(ClaimsPrincipal claimsPrincipal, string type)
        {
            try
            {
                var getClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == type);
                if (getClaim != null)
                    return getClaim.Value;
                else
                    return string.Empty;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetClaimValue() of {type} failed. Exception Error: {e.Message}");
                return string.Empty;
            }
        }
    }
}
