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

        public async Task<bool> IsVolumeAvailable()
        {
            


            return true;
        }
    }
}
