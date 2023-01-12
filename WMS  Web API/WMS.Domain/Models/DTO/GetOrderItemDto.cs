using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class GetOrderItemDto
    {
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

    }
}
