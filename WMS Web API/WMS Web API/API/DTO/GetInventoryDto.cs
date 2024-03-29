﻿namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get inventory item data transfer object
    /// </summary>
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
        public string WarehouseName { get; set; } = string.Empty;

        /// <summary>
        /// Inventory item product name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Inventory item product SKU
        /// </summary>
        public string ProductSKU { get; set; } = string.Empty;

        /// <summary>
        /// Inventory item product description
        /// </summary>
        public string ProductDescription { get; set; } = string.Empty;
    }
}
