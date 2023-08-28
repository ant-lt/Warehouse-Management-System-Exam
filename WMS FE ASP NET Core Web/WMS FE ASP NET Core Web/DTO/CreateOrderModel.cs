namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class CreateOrderModel
    {
        /// <summary>
        /// Order processing scheduled date
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Order processing actual execution date
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Order status Id
        /// </summary>
        public int OrderStatusId { get; set; }

        /// <summary>
        /// Order type Id
        /// </summary>
        public int OrderTypeId { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public int WMSuserId { get; set; }
    }
}
