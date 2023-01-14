using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("Orders")]
    public class Order
    {

        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        
        public DateTime? ScheduledDate { get; set; }
        
        public DateTime? ExecutionDate { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        [ForeignKey("OrderStatusId")] 
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        
        [Required]
        public int OrderTypeId { get; set; }

        [ForeignKey("OrderTypeId")]
        public virtual OrderType OrderType { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")] 
        public virtual Customer Customer { get; set; } = null!;

        [Required]
        public int WMSuserId { get; set; }

        [ForeignKey("WMSuserId")]
        public virtual WMSuser RWMSuser { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
