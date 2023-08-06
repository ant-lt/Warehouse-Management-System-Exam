namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class WarehousesRatioOfOccupiedModel
    {
        /// <summary>
        /// Warehouse id in the system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Warehouse name where are stored inventory item
        /// </summary>
        public string WarehouseName { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse description
        /// </summary>
        public string WarehouseDescription { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse location
        /// </summary>
        public string WarehouseLocation { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse total volume capacity
        /// </summary>
        public double WarehouseTotalVolumeCapacity { get; set; }

        /// <summary>
        /// Warehouse actual total occupied volume
        /// </summary>
        public double WarehouseActualTotalOccupiedVolume { get; set; }


        /// <summary>
        /// Warehouse available total volume
        /// </summary>
        public double WarehouseAvailableTotalVolume { get; set; }
    }
}
