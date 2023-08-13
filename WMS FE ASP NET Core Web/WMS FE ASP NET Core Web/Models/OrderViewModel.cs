using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class OrderViewModel
    {
        [Required]
        public OrderModel? Order { get; set; } 

        public List<OrderItemModel>? OrderItems { get; set; } 

        public double TotalVolume { get; set; }
    }
}
