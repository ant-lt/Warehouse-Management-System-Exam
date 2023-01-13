using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Models.DTO
{
    public class CreateOrderDto
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
        [Required]
        public int OrderStatusId { get; set; }

        /// <summary>
        /// Order type Id
        /// </summary>
        [Required]
        public int OrderTypeId { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public int WMSuserId { get; set; }

    }
}
