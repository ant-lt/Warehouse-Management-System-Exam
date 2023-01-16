using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;

namespace WMS.Infastructure.Interfaces
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<double> TotalVolumeAvailableAsync();
        Task<int?> WarehouseIdFitToFillAsync(double totalOrderVolume);
        Task<double> GetWarehouseRatioOfOccupiedbyIdAsync(int WarehouseId);
        Task<List<Warehouse>?> GetWarehouseListAsync();
        Task<double> GetOrderTotalVolumeAsync(int orderId);
        Task<string> GetOrderCurrentStatusAsync(int orderId);
        Task<bool> TransferOrderItemsToWarehouseAsync(int orderId, int warehouseId);
        Task<bool> TransferOrderItemsFromWarehouseAsync(int orderId);
        Task<string> GetOrderTypeNameAsync(int orderId);
        Task<bool> ChangeOrderStatusAsync(int orderId, string statusName);
    }
}
