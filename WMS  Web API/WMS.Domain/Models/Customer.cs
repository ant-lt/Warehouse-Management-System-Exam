using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string LegalCode { get; set; } = null!;

        [Required]
        [MaxLength(100)] 
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(20)] 
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PostCode { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string? ContactPerson { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        public DateTime? Created { get; set; } = DateTime.Now;

    }
}
