using WMS.Domain.Models;

namespace WMS.Infastrukture.Database.InitialData
{
    public static class OrderTypeInitialData
    {
        public static readonly OrderType[] orderTypeInitialDataSeed = new OrderType[]
        {
            new OrderType
            {
                Id= 1,
                Name = "Inbound",
                Description = "Receiving of goods"
            },
            new OrderType
            { 
                Id= 2,
                Name = "Outbound",
                Description = "Moving inventory out of warehouse"
            }
        };
    }
}
