using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WMS.Infastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly WMSContext _db;

        public OrderRepository(WMSContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<OrderItem>> GetOrderItemsByIdAsync(int orderId)
        {
            IQueryable<OrderItem> orderItems = _db.OrderItems.Where(e => e.OrderId == orderId);

            return await orderItems.ToListAsync();
        }
    }
}
