using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class GetShipmentDto
    {

        public int Id { get; set; }

        public DateTime Date { get; set; } 

        public DateTime? ScheduledDate { get; set; }

        public DateTime? ExecutionDate { get; set; }

        public int ShipmentStatusId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int WMSuserId { get; set; }

    }
}
