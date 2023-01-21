using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;
using WMS.Infastructure.Repositories;

namespace WMS.Infastructure.Services
{
    public class InventoryManagerService : IInventoryManagerService
    {
        private readonly IInventoryRepository _repository;

        public InventoryManagerService(IInventoryRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        ///  Initiate the order processing
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<bool> ProcessOrderAsync(int orderId)
        {

            string orderStatus = await _repository.GetOrderCurrentStatusAsync(orderId);


            if ( orderStatus != "New")
            {
                return false;
            }

            string orderType = await _repository.GetOrderTypeNameAsync(orderId);

            if ( orderType == "Inbound")
            {

                var orderTotalVolume = await _repository.GetOrderTotalVolumeAsync(orderId);

                var availableVolume = await _repository.TotalVolumeAvailableAsync();


                // Check if orders items total volume are fit to warehouses
                if (orderTotalVolume <= availableVolume)
                {
                    // Here is the main point of program - select the warehouse with are fit the order total volume
                    var wareHouseAssigned = await _repository.WarehouseIdFitToFillAsync(orderTotalVolume);


                    if (wareHouseAssigned == null)
                    {
                        // No space available on any warehouses

                        await _repository.ChangeOrderStatusAsync(orderId, "Canceled");

                        return false;
                    }
    
                    var ordersItems = await _repository.TransferOrderItemsToWarehouseAsync(orderId, (int)wareHouseAssigned);

                    var changeOrderStatus = await _repository.ChangeOrderStatusAsync(orderId, "Completed");

                    return changeOrderStatus;
                }


            }
            else if (orderType == "Outbound")
            {
                var ordersItems = await _repository.TransferOrderItemsFromWarehouseAsync(orderId);
                var changeOrderStatus = await _repository.ChangeOrderStatusAsync(orderId, "Completed");

                return changeOrderStatus;
            }
            return false;
        }
    }
}
