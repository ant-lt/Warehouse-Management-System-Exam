namespace WMS_FE_ASP_NET_Core_Web.DTO
{
    public class UpdateCustomerModel
    {
        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Customer company legal code
        /// </summary>
        public string LegalCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer company legal address
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Customer address city
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Customer address post code
        /// </summary>
        public string PostCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer address country
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Customer contact person name
        /// </summary>
        public string? ContactPerson { get; set; }

        /// <summary>
        /// Customer contact person phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Customer contact person e-mail address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Customer status Active/inactive in Warehouse system
        /// </summary>
        public bool Status { get; set; }

    }
}
