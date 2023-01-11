using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class UpdateOrderDto
    {
        public DateTime? ScheduledDate { get; set; }

        public DateTime? ExecutionDate { get; set; }

        public int OrderStatusId { get; set; }

    }
}
