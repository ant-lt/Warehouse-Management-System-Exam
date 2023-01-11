namespace WMS.Domain.Models.DTO
{
    public class CreateOrderDto
    {

        public DateTime? ScheduledDate { get; set; }

        public DateTime? ExecutionDate { get; set; }
  
        public int OrderStatusId { get; set; }

        public int OrderTypeId { get; set; }

        public int CustomerId { get; set; }

        public int WMSuserId { get; set; }

    }
}
