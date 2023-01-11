﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class UpdateCustomerDto
    {
      

        public string Name { get; set; }


        public string LegalCode { get; set; }


        public string Address { get; set; }


        public string City { get; set; }


        public string PostCode { get; set; }


        public string Country { get; set; }


        public string? ContactPerson { get; set; }


        public string? PhoneNumber { get; set; }


        public string? Email { get; set; }

        public bool Status { get; set; }


    }
}
