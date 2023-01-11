namespace WMS.Domain.Models.DTO
{
    public class GetInventoryDto
    {
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        public int WarehouseId { get; set; }

        public int ProductId { get; set; }
    }
}
