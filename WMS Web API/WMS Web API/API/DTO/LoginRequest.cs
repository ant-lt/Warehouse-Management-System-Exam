using System.ComponentModel.DataAnnotations;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Login request data transfer object
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// username field for the login request
        /// </summary>
        [Required]        
        public string? Username { get; set; }

        /// <summary>
        /// password field for the login request
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
