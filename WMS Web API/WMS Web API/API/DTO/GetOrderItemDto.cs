namespace WMS_Web_API.API.DTO
{
    public class GetOrderItemDto
    {
        /// <summary>
        /// Order item internal Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Order item product quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Order item product SKU
        /// </summary>
        public string ProductSKU { get; set; }

        /// <summary>
        /// Order item product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Order item product description
        /// </summary>
        public string ProductDescription { get; set; }
    }
}
