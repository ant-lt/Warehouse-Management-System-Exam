using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("Shipments")]
    public class Shipment
    {
        public Shipment()
        {
            ShipmentItems = new HashSet<ShipmentItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        public DateTime? ScheduledDate { get; set; }

        public DateTime? ExecutionDate { get; set; }

        [Required]
        public int ShipmentStatusId { get; set; }

        [ForeignKey("ShipmentStatusId")]
        public virtual ShipmentStatus ShipmentStatus { get; set; } = null!;

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [Required]
        public int WMSuserId { get; set; }

        [ForeignKey("WMSuserId")]
        public virtual WMSuser RWMSuser { get; set; } = null!;

        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }

    }
}
