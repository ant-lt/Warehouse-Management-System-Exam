﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class CreateCustomerDto
    {

        [MaxLength(100)]
        public string Name { get; set; } 

        [MaxLength(20)]
        public string LegalCode { get; set; }

 
        [MaxLength(100)]
        public string Address { get; set; } 

   
        [MaxLength(20)]
        public string City { get; set; }

    
        [MaxLength(20)]
        public string PostCode { get; set; }

   
        [MaxLength(20)]
        public string Country { get; set; } 

  
        [MaxLength(50)]
        public string? ContactPerson { get; set; }

     
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

 
        [MaxLength(100)]
        public string? Email { get; set; }

    }
}
