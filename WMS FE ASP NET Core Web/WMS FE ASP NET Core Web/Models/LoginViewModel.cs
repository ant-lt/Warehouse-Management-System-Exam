using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Too Long")]
        public string Message { get; set; }
    }
}
