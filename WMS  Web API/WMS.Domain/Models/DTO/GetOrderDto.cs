using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class GetOrderDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } 

        public DateTime? ScheduledDate { get; set; }

        public DateTime? ExecutionDate { get; set; }

        public string OrderStatus { get; set; }

        public string OrderType { get; set; }

        public string CustomerName { get; set; }

        public string CreatedByUser { get; set; }

    }
}
