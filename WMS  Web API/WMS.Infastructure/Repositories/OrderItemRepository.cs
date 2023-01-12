using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly WMSContext _db;

        public OrderItemRepository(WMSContext db) : base(db)
        {
            _db = db;    
        }

    }
}
