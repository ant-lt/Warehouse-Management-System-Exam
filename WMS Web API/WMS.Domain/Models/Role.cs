using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Models
{
    [Table("Roles")]
    public class Role
    {
        public Role() 
        {
            WMSusers = new HashSet<WMSuser>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)] 
        public string Description { get; set; } = null!;


        public virtual ICollection<WMSuser> WMSusers { get; set; }
    }
}
