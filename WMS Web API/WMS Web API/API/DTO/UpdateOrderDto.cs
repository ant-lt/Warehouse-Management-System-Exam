﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Update order data transfer object
    /// </summary>
    public class UpdateOrderDto
    {
        /// <summary>
        /// Order processing scheduled date
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Order processing actual execution date
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Order status Id
        /// </summary>
        [Required]
        public int OrderStatusId { get; set; }

        /// <summary>
        /// Order type Id
        /// </summary>
        [Required]
        public int OrderTypeId { get; set; }
    }
}