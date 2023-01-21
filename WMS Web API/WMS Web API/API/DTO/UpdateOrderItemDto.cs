
using System.ComponentModel.DataAnnotations;

namespace WMS_Web_API.API.DTO
{
    public class UpdateOrderItemDto
    {
        /// <summary>
        /// Product quantity
        /// </summary>
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Order Item Product Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
    }
}
