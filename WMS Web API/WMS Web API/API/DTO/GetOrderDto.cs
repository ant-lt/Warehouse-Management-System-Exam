using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get Order data transfer object
    /// </summary>
    public class GetOrderDto
    {
        /// <summary>
        /// Internal Order Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Order submit date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Order processing scheduled date
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Order processing actual execution date
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Order actual status
        /// </summary>
        public string OrderStatus { get; set; } = string.Empty;

        /// <summary>
        /// Order type
        /// </summary>
        public string OrderType { get; set; } = string.Empty;

        /// <summary>
        /// Order customer name
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// User name created the order
        /// </summary>
        public string CreatedByUser { get; set; } = string.Empty;
    }
}