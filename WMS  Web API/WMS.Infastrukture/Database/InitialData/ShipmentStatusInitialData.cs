using WMS.Domain.Models;

namespace WMS.Infastrukture.Database.InitialData
{
    public static class ShipmentStatusInitialData
    {
        public static readonly ShipmentStatus[] shipmentStatusInitialDataSeed = new ShipmentStatus[]
        {
            new ShipmentStatus
            {
                Id= 1,
                Name = "Arrived",
                Description = "Goods arrived to warehouse"
            },
            new ShipmentStatus
            { 
                Id= 2,
                Name = "Loading Complete",
                Description = "Warehouse loading of the goods complete"
            },
            new ShipmentStatus
            {
                Id= 3,
                Name = "Dispached",
                Description = "Goods dispached from warehouse"
            },
            new ShipmentStatus
            {
                Id= 4,
                Name = "Dispatching Complete",
                Description = "Goods dispatching complete"
            },
            new ShipmentStatus
            {
                Id= 5,
                Name = "Awaiting Arrival",
                Description = "Awaiting goods arrival"
            },
            new ShipmentStatus
            {
                Id= 6,
                Name = "Awaiting Dispatch",
                Description = "Awaiting goods dispatch"
            }
        };
    }
}
