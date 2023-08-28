using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class OrderType
    {
        /// <summary>
        /// Order type Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Order type name
        /// </summary>
        [Required]
        public string OrderTypeName { get; set; } = string.Empty;
    }
}
