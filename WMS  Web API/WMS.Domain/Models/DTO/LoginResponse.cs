namespace WMS.Domain.Models.DTO
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public bool Active { get; set; }
        public string Token { get; set; }
    }
}
