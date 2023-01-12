using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class GetShipmentItemDto
    {
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        public int ShipmentId { get; set; }

        public int ProductId { get; set; }

    }
}
