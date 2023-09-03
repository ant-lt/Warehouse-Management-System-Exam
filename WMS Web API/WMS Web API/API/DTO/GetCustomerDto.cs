
namespace WMS_Web_API.API.DTO
{
    /// <summary>
    /// Get customer data transfer object
    /// </summary>
    public class GetCustomerDto
    {
        /// <summary>
        /// Customer Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Customer company legal code
        /// </summary>
        public string LegalCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer address
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Customer address city
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Customer post code
        /// </summary>
        public string PostCode { get; set; } = string.Empty;

        /// <summary>
        /// Customer country name
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
        /// Customer e-mail address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Customer status
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Customer creation date
        /// </summary>
        public DateTime? Created { get; set; } 
    }
}