public class OrderItemModel
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public string ProductSKU { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public double Volume { get; set; }
    public int ProductId { get; set; }
}
