public class OrderModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string OrderStatus { get; set; } = "New";
    public string OrderType { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CreatedByUser { get; set; } = string.Empty;
}
