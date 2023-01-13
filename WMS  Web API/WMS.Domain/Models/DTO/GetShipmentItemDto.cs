﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class GetShipmentItemDto
    {
        /// <summary>
        /// Shipment item internal Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Shipment item product quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Shipment Id
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// Shipment item product SKU
        /// </summary>
        public string ProductSKU { get; set; }

        /// <summary>
        /// Shipment item product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Shipment item product description
        /// </summary>
        public string ProductDescription { get; set; }
    }
}
