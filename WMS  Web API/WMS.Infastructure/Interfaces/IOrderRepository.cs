using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;

namespace WMS.Infastructure.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<OrderItem>> GetOrderItemsByIdAsync(int orderId);
    }
}
