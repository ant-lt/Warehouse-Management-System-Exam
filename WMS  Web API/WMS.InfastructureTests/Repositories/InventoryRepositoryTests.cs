using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.Infastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Infastructure.Database;
using Microsoft.Extensions.Hosting;
using WMS.Domain.Models;
using Castle.Core.Resource;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories.Tests
{

    [TestClass()]
    public class InventoryRepositoryTests
    {
        private WMSContext _dbcontext;

        [TestInitialize]
        public void OnInit()
        {
            var options = new DbContextOptionsBuilder<WMSContext>()
                .UseInMemoryDatabase(databaseName: "WMSDb.db")
                .Options;
            _dbcontext = new WMSContext(options);

            _dbcontext.Warehouses.AddRange(new Warehouse[]
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
            });

            _dbcontext.Products.AddRange(new Product[]
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
            });


            _dbcontext.OrderStatuses.AddRange(new OrderStatus[]
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
             });

            _dbcontext.Customers.AddRange(new Customer[]
             {
                new Customer
                {
                    Id= 1,
                    Name = "UAB Bandymas",
                    LegalCode = "123456789",
                    Address = "Testavimo g. 1",
                    City = "Vilnius",
                    PostCode = "LT-12345",
                    Country = "Lietuva",
                    ContactPerson = "Contact Person",
                    PhoneNumber = "Phone Number",
                    Email = "test@test.com",
                    Status = true,
                    Created = DateTime.Now
                }
             });

            _dbcontext.Orders.AddRange(new Order[]
            {
                new Order
                {
                    Id= 1,
                    Date = DateTime.Now,
                    OrderStatusId = 1,
                    OrderTypeId = 1,
                    CustomerId = 1,
                    WMSuserId = 1,
                }
            });



            _dbcontext.Inventories.Add(new Inventory { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1 });

            _dbcontext.SaveChanges();
        }

        [TestMethod()]
        public void TotalVolumeAvailableAsyncTest()
        {
            IInventoryRepository inventoryRepository = new InventoryRepository(_dbcontext);

            var totalVolumeAvailable = inventoryRepository.TotalVolumeAvailableAsync().Result;

            double excpected = 599.976707625;

            Assert.AreEqual(excpected, totalVolumeAvailable);

        }

        [TestMethod()]
        public void WarehouseIdFitToFillAsyncTest()
        {

            IInventoryRepository inventoryRepository = new InventoryRepository(_dbcontext);

            var warehouseIdtofit = inventoryRepository.WarehouseIdFitToFillAsync(20).Result;

            int excpectedWarehouseId = 1;

            Assert.AreEqual(excpectedWarehouseId, warehouseIdtofit);

        }


        [TestMethod()]
        public void GetOrderTotalVolumeAsyncTest()
        {

            IInventoryRepository inventoryRepository = new InventoryRepository(_dbcontext);


            var orderTotalVolume = inventoryRepository.GetOrderTotalVolumeAsync(1).Result;

            double excpectedOrderVolume = 0;

            Assert.AreEqual(excpectedOrderVolume, orderTotalVolume);


        }

    }
}