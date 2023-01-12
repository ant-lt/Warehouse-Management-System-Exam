using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class CreateOrderItemDto
    {

        public decimal Quantity { get; set; }
        
        public int OrderId { get; set; }

        public int ProductId { get; set; }

    }
}
