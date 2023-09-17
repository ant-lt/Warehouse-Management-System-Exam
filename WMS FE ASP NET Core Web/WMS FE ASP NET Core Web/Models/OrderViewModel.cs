using System.ComponentModel.DataAnnotations;
using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class OrderViewModel
    {
        [Required]
        public OrderModel? Order { get; set; }
        
        [Display(Name = "Products")]
        public int ProductId { get; set; }
        public List<ProductModel>? Products { get; set; } = new List<ProductModel>();

        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Quantity { get; set; }

        [Display(Name = "Order Items")]
        public List<OrderItemModel>? OrderItems { get; set; } 

        public double TotalVolume { get; set; }
    }
}
