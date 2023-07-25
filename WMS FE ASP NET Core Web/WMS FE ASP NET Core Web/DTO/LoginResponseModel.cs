namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class LoginResponseModel
    {
        public string UserName { get; set; }
        public bool Active { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }
}
