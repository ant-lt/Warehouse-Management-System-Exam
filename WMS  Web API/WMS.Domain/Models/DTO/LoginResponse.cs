namespace WMS.Domain.Models.DTO
{
    public class LoginResponse
    {
        /// <summary>
        /// Login user name
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// Actual user status Active/Inactive in Warehouse management system
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// User connection token
        /// </summary>
        public string Token { get; set; }


        /// <summary>
        /// User role name in Warehouse management system
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// User internal Id on Warehouse management system
        /// </summary>
        public int UserId { get; set; }
    }
}
