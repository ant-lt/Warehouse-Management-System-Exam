namespace WMS.Domain.Models
{
    public class LocalUser
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } 
  
    }
}
