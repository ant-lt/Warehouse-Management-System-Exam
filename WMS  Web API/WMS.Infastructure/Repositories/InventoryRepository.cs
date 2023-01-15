﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly WMSContext _db;

        public InventoryRepository(WMSContext db) : base(db)
        {
            _db = db;
        }

        
        /// <summary>
        /// Get total of volume available on all warehouses
        /// </summary>
        /// <returns>Total available volume</returns>
        public async Task<double> TotalVolumeAvailableAsync()
        {

            double result = 0;
            var allWarehouseVolume = await _db.Warehouses.SumAsync(x => (double)x.TotalVolume);

            var allOccupiedInventoriesVolume = await _db.Inventories
            .Include(x => x.Product)
            .Select(x => new
            {
                VolumeOccupied = (double)x.Quantity * (double)x.Product.Volume
            }
            )
            .SumAsync(x => x.VolumeOccupied);

            result = allWarehouseVolume - allOccupiedInventoriesVolume;

            return result;     
        }

        /// <summary>
        /// Check the new order total volume are fit to warehouse
        /// </summary>
        /// <param name="totalOrderVolume"></param>
        /// <returns>Warehouse Id to fit new order</returns>
        public async Task<int?> WarehouseIdFitToFillAsync(double totalOrderVolume)
        {

            // Warehouse Volume callculations for all inventories
            var allInventoriesTotalVolumeCalculation = await _db.Inventories
            .Include(x => x.Product)
            .Select(x => new
            {
                WarehouseId = x.WarehouseId,
                VolumeOccupied = (double)x.Quantity * (double)x.Product.Volume
            }
             )
            .GroupBy(x => x.WarehouseId).Select(item => new
                        {
                            WarehouseId = item.Key,
                            WarehouseOccupiedVolume = item.Sum(x => x.VolumeOccupied),
            })
            .ToListAsync();


            // get all warehouses ratio of occupied 
            var totalVolumeOccupietyWarehouse = (from a in await GetWarehouseListAsync()
                                                 from b in allInventoriesTotalVolumeCalculation.Where(b => b.WarehouseId == a.Id).DefaultIfEmpty()
                        select new { 
                            WarehouseId = a.Id,
                            WarehouseTotalVolume = a.TotalVolume,
                            TotalOccupiedVolume = b?.WarehouseOccupiedVolume != null ? b.WarehouseOccupiedVolume : 0
                        }).ToList();

            // Find the warehouse with the most available volume
            // if warehouse is null - no any space on any warehouse
            
            var warehouse = totalVolumeOccupietyWarehouse
                .Where(w => ((double)w.WarehouseTotalVolume - (double)w.TotalOccupiedVolume) >= totalOrderVolume)
                .OrderByDescending(w => w.TotalOccupiedVolume)
                .FirstOrDefault();
           
                return warehouse?.WarehouseId;
        }

        /// <summary>
        /// Get Warehouse ratio of occupied  by warehouse Id
        /// </summary>
        /// <param name="WarehouseId"></param>
        /// <returns>Total occupied volume</returns>
        public async Task<double> GetWarehouseRatioOfOccupiedbyIdAsync(int WarehouseId)
        {
            
            var totalVolumeByWarehouseId = await _db.Inventories
            .Include(x => x.Product)
            .Where(x => x.WarehouseId == WarehouseId)
            .Select(x => new
            {
                VolumeOccupied = (double)x.Quantity * (double)x.Product.Volume
            }
             )
            .SumAsync( x => x.VolumeOccupied);


            return totalVolumeByWarehouseId;
        }

        /// <summary>
        /// Get list of all warehouses
        /// </summary>
        /// <returns>List of all warehouses</returns>
        public async Task<List<Warehouse>?> GetWarehouseListAsync()
        {
            return await _db.Warehouses.Select(s => s).ToListAsync();
        }
    }
}
