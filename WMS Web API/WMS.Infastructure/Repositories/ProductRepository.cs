﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly WMSContext _db;
        
        public ProductRepository(WMSContext db) : base(db)
        {
            _db = db;
        }

    }
}
