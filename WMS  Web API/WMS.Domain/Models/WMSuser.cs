using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Models
{
    [Table("WMSusers")]
    public class WMSuser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)] 
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)] 
        public string Name { get; set; } = null!;
        
        public byte[] PasswordHash { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;

        [Required]
        public bool Active { get; set; } = false;
        
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } 
  
    }
}
