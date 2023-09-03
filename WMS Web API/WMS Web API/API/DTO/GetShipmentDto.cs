namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// GetShipmentDto is a data transfer object used to get shipment data from WMS
    /// </summary>
    public class GetShipmentDto
    {

        /// <summary>
        /// Internal Shipment Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Shipment submit date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Shipment processing scheduled date
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// Shipment processing actual execution date
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Shipment status
        /// </summary>
        public string ShipmentStatus { get; set; } = string.Empty;

        /// <summary>
        /// Order id related to shipment
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Shipment customer name
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Shipment created by user name
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}