using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("OrderTypes")]
    public class OrderType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] 
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)] 
        public string Description { get; set; } = null!;
    }
}
