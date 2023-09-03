﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Create customer data transfer object
    /// </summary>
    public class CreateCustomerDto
    {

        /// <summary>
        /// Customer name
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Customer company legal code
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "Company legal code cannot be longer than 20 characters")]
        public string LegalCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer company legal address
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Customer address city
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "City cannot be longer than 20 characters")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Customer address post code
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "Post code cannot be longer than 20 characters")]
        public string PostCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer address country
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "Country name cannot be longer than 20 characters")]
        public string Country { get; set; } = string.Empty;
  
        /// <summary>
        /// Customer contact person name
        /// </summary>
        [MaxLength(50, ErrorMessage = "Contact person name cannot be longer than 50 characters")]
        public string? ContactPerson { get; set; }

        /// <summary>
        /// Customer contact person phone number
        /// </summary>
        [MaxLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Customer contact person e-mail address
        /// </summary>
        [MaxLength(100, ErrorMessage = "E-mail cannot be longer than 100 characters")]
        public string? Email { get; set; }

    }
}