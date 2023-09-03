using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get order types data transfer object
    /// </summary>
    public class GetOrderTypesDto
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
