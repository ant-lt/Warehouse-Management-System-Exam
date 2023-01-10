using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)] public string SKU { get; set; } = null!; 

        [Required]
        [MaxLength(50)] public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)] public string Description { get; set; } = null!;

        [Required]
        public decimal Volume { get; set; }

        [Required]
        public decimal Weigth { get; set; }
    }
}
