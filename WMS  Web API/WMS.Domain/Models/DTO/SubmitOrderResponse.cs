using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models.DTO
{
    public class SubmitOrderResponse
    {
        /// <summary>
        /// Submited Order Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Submited Order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Submited Order status
        /// </summary>
        public string OrderStatus { get; set; }
    }
}
