using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Infastructure.Interfaces
{
    public interface IInventoryManagerService
    {
        Task<bool> IsVolumeAvailable();
    }
}
