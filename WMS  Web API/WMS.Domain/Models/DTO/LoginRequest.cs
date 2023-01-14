using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Models.DTO
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
