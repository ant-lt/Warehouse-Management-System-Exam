using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories 
{
    public class ShipmentItemRepository : Repository<ShipmentItem>, IShipmentItemRepository
    {
        private readonly WMSContext _db;

        public ShipmentItemRepository(WMSContext db) : base(db)
        {
            _db = db;
        }
    }
}
