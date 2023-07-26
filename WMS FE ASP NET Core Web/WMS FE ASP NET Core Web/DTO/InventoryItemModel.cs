namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class InventoryItemModel
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty; 
        public string ProductDescription { get; set; } = string.Empty;
    }
}
