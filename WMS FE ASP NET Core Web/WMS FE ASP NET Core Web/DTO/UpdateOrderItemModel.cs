using System.ComponentModel.DataAnnotations;

namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    /// <summary>
    /// Update Order Item Model
    /// </summary>
    public class UpdateOrderItemModel
    {
        /// <summary>
        /// Product quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Order Item Product Id
        /// </summary>
        public int ProductId { get; set; }
    }
}
