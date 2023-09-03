namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Login response data transfer object
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Login user name
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Actual user status Active/Inactive in Warehouse management system
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// User connection token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// User role name in Warehouse management system
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// User internal Id on Warehouse management system
        /// </summary>
        public int UserId { get; set; }
    }
}