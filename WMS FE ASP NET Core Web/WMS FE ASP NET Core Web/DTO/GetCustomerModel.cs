namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class GetCustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LegalCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime? Created { get; set; }
    }
}
