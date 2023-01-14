using WMS.Domain.Models;

namespace WMS.Infastructure.Database.InitialData
{
    public static class ProductInitialData
    {
        public static readonly Product[] productInitialDataSeed = new Product[]
        {
            new Product
            {
                Id= 1,
                SKU = "A-000001",
                Name = "Epson ECOTANK L3256",
                Description = "Daugiafunkcis spausdintuvas",
                Volume = 0.023292375M,
                Weigth= 3.9M
            },
            new Product
            { 
                Id= 2,
                SKU = "A-000002",
                Name = "Kyocera Ecosys M5526CDW",
                Description = "Daugiafunkcis spausdintuvas",
                Volume = 0.0903M,
                Weigth= 26
            },
            new Product
            {
                Id= 3,
                SKU = "A-000003",
                Name = "Samsung QE55Q60BAUXXH, QLED, 55",
                Description = "Televizorius",
                Volume = 0.026199M, 
                Weigth = 15.8M
            },
            new Product
            {
                Id= 4,
                SKU = "A-000004",
                Name = "Whirlpool SP40 802 EU 2",
                Description = "Įmontuojamas šaldytuvas",
                Volume = 0.7257765M,
                Weigth = 74
            },
            new Product
            {
                Id= 5,
                SKU = "A-000005",
                Name = "Gorenje Advanced Line GEIT5C60BPG",
                Description = "Indukcinė viryklė su elektrine orkaite",
                Volume = 0.25075M,
                Weigth = 42
            },
            new Product
            {
                Id= 6,
                SKU = "A-000006",
                Name = "Electrolux EEM69410L",
                Description = "Įmontuojama indaplovė",
                Volume = 0.262845M,
                Weigth = 38.39M
            },
            new Product
            {
                Id= 7,
                SKU = "A-000007",
                Name = "Beko WUE6511BS, 6 kg, balta",
                Description = "Skalbimo mašina",
                Volume = 0.22176M,
                Weigth = 35M
            },
            new Product
            {
                Id= 8,
                SKU = "A-000008",
                Name = "Samsung DV90T6240LE/S7",
                Description = "Džiovyklė",
                Volume = 0.306M,
                Weigth = 50M
            },
            new Product
            {
                Id= 9,
                SKU = "A-000009",
                Name = "Faber Flexa NG AM/XA60",
                Description = "Įmontuojamas gartraukis",
                Volume = 0.03024M,
                Weigth = 7.5M
            },
            new Product
            {
                Id= 10,
                SKU = "A-000010",
                Name = "Beko RFNE312E43XN",
                Description = "Šaldiklis",
                Volume = 0.7215M,
                Weigth = 70M
            }
        };
    }
}
