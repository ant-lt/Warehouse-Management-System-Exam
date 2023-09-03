namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get product data transfer object
    /// </summary>
    public class GetProductDto
    {
        /// <summary>
        /// Internal product Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product SKU
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product volume of 1 pcs.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Product weigth of 1 pcs.
        /// </summary>
        public decimal Weigth { get; set; }
    }
}