using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Infastructure.Database.InitialData;

namespace WMS.Infastructure.Database
{
    public class WMSContext : DbContext
    {
    
        
        public WMSContext(DbContextOptions<WMSContext> options) : base(options)
        {

        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WMSuser> WMSusers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string ConnectionString;
                // %LOCALAPPDATA%
               // var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.CurrentDirectory;

                //var path = Environment.GetFolderPath(folder);

                ConnectionString = Path.Join(path, "WMSDb.db");
                optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(RoleInitialData.rolesInitialDataSeed);
            modelBuilder.Entity<OrderStatus>().HasData(OrderStatusInitialData.orderStatusInitialDataSeed);
            modelBuilder.Entity<OrderType>().HasData(OrderTypeInitialData.orderTypeInitialDataSeed);
            modelBuilder.Entity<ShipmentStatus>().HasData(ShipmentStatusInitialData.shipmentStatusInitialDataSeed);
            modelBuilder.Entity<Customer>().HasData(CustomerInitialData.customerInitialDataSeed);
            modelBuilder.Entity<Warehouse>().HasData(WarehouseInitialData.warehouseInitialDataSeed);
            modelBuilder.Entity<Product>().HasData(ProductInitialData.productInitialDataSeed);
        }
    }
}
