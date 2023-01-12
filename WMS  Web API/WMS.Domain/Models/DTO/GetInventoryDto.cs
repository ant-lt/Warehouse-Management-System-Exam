namespace WMS.Domain.Models.DTO
{
    public class GetInventoryDto
    {
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        public string WarehouseName { get; set; }

        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public string ProductDecription { get; set; }
    }
}
