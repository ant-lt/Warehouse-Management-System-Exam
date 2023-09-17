namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class CreateOrderItemModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
