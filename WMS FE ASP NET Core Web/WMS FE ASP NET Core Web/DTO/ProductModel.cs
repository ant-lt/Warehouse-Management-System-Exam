namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Volume { get; set; }
        public double Weight { get; set; }
    }
}
