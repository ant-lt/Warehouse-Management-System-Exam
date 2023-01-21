using System.ComponentModel.DataAnnotations;

namespace WMS_Web_API.API.DTO
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
