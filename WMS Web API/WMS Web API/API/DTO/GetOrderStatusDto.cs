using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get order status data transfer object
    /// </summary>
    public class GetOrderStatusDto
    {
        /// <summary>
        /// Order status Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Order status name
        /// </summary>
        [Required]
        public string OrderStatusName { get; set; } = string.Empty;
    }
}
