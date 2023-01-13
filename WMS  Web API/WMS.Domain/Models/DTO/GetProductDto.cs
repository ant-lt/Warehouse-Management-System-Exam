namespace WMS.Domain.Models.DTO
{
    public class GetProductDto
    {

        /// <summary>
        /// Internal product Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product SKU
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

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
