namespace WMS.Domain.Models.DTO
{
    public class GetInventoryDto
    {

        /// <summary>
        /// Internal inventory Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Inventory actual quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Warehouse name where are stored inventory item
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// Inventory item product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Inventory item product SKU
        /// </summary>
        public string ProductSKU { get; set; }

        /// <summary>
        /// Inventory item product description
        /// </summary>
        public string ProductDescription { get; set; }
    }
}
