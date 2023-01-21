using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("Warehouses")]
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] public string Name { get; set; } = null!;

        [Required]
        [MaxLength(50)] public string Description { get; set; } = null!;

        [Required]
        [MaxLength(50)] public string Location { get; set; } = null!;
        
        [Required]
        public decimal TotalVolume { get; set; }

        [Required]
        public decimal TotalWeigth { get; set; }

        public virtual ICollection<Inventory> InventoryItems { get; set; }
    }
}
