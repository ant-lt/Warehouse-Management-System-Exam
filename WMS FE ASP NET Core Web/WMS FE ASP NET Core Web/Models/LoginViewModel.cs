using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
