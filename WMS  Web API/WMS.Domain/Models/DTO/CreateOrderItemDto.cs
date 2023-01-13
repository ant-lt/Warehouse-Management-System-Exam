using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class CreateOrderItemDto
    {

        /// <summary>
        /// Product quantity
        /// </summary>
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Order Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Order Item Product Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }

    }
}
