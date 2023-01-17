﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
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
        public string OrderTypeName { get; set; }
    }
}
