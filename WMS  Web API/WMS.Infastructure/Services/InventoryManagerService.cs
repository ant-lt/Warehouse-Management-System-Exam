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


        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            
            var orderTotalVolume = await _repository.GetOrderTotalVolumeAsync(orderId);

            var availableVolume = await _repository.TotalVolumeAvailableAsync();


            if (orderTotalVolume <= availableVolume)
            {

                string orderStatus = await _repository.GetOrderCurrentStatusAsync(orderId);

                var wareHouseAssigned = await _repository.WarehouseIdFitToFillAsync(orderTotalVolume);

                if (wareHouseAssigned == null || orderStatus != "New") { 
                    return false;
                }

                var ordersItems = await _repository.TransferOrderItemsToWarehouseAsync(orderId, (int)wareHouseAssigned);


                return true;
            }


            return false;
        }
    }
}
