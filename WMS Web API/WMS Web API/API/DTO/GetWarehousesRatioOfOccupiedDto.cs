using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get warehouses ratio of occupied data transfer object
    /// </summary>
    public class GetWarehousesRatioOfOccupiedDto
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