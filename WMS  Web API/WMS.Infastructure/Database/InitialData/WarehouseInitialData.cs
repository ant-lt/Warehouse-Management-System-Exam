using WMS.Domain.Models;

namespace WMS.Infastructure.Database.InitialData
{
    public static class WarehouseInitialData
    {
        public static readonly Warehouse[] warehouseInitialDataSeed = new Warehouse[]
        {
            new Warehouse
            {
                Id= 1,
                Name = "Sandėlis Nr.1",
                Location = "Vilnius, Sandėlių gatve",
                Description = "Sektorius Nr.1",
                TotalVolume = 100,
                TotalWeigth= 200
            },
            new Warehouse
            { 
                Id= 2,
                Name = "Sandėlis Nr.2",
                Location = "Vilnius, Sandėlių gatve",
                Description = "Sektorius Nr.2",
                TotalVolume = 200,
                TotalWeigth= 250
            },
            new Warehouse
            {
                Id= 3,
                Name = "Sandėlis Nr.3",
                Location = "Vilnius, Sandėlių gatve",
                Description = "Sektorius Nr.3",
                TotalVolume = 300,
                TotalWeigth= 400
            }
        };
    }
}
