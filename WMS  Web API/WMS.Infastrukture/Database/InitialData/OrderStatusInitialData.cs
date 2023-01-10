using WMS.Domain.Models;

namespace WMS.Infastrukture.Database.InitialData
{
    public static class OrderStatusInitialData
    {
        public static readonly OrderStatus[] orderStatusInitialDataSeed = new OrderStatus[]
        {
            new OrderStatus
            {
                Id= 1,
                Name = "New",
                Description = "New Order Ready"
            },
            new OrderStatus
            { 
                Id= 2,
                Name = "Completed",
                Description = "Order Completed"
            },
            new OrderStatus
            {
                Id= 3,
                Name = "Canceled",
                Description = "Order Canceled"
            }
        };
    }
}
