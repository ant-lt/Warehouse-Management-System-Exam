﻿using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class RegistrationRequestModel
    {
        /// <summary>
        /// User registration user name
        /// </summary>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// User registration name
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string? Password { get; set; }

        /// <summary>
        /// User role name in Warehouse management system
        /// </summary>
        [Required]
        public string? Role { get; set; }
    }
}
