using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.Models
{
    public class OrderItemViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Display(Name = "Product SKU")]
        public string ProductSKU { get; set; } = string.Empty;

        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; } = string.Empty;

        [Display(Name = "Quantity")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public double Quantity { get; set; }

        [Display(Name = "Volume")]
        public double Volume { get; set; }
    }
}
