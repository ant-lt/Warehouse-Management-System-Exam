using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
    {
        private readonly WMSContext _db;

        public ShipmentRepository(WMSContext db) : base(db)
        {
            _db= db;
        }

        public async Task<List<ShipmentItem>> GetShipmentItemsByIdAsync(int shipmentId)
        {
            IQueryable<ShipmentItem> shipmentItems = _db.ShipmentItems.Where(e => e.ShipmentId == shipmentId);

            return await shipmentItems.ToListAsync();

        }


    }
}
