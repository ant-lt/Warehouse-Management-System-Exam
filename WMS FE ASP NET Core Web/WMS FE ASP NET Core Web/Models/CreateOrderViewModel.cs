using System.ComponentModel.DataAnnotations;
using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class CreateOrderViewModel
    {
        [Required]        
        public DateTime? ScheduledDate { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        [Display(Name = "Type of order")]
        public int OrderTypeId { get; set; }
        public List<OrderType>? OrderTypes { get; set; } = new List<OrderType>();

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public List<CustomerModel>? Customers { get; set; } = new List<CustomerModel>();

        [Display(Name = "Order Items")]
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
