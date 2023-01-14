
namespace WMS.Domain.Models.DTO
{
    public class GetCustomerDto
    {
        /// <summary>
        /// Customer Id on Warehouse management system
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Customer company legal code
        /// </summary>
        public string LegalCode { get; set; }


        /// <summary>
        /// Customer address
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Customer address city
        /// </summary>
        public string City { get; set; }


        /// <summary>
        /// Customer post code
        /// </summary>
        public string PostCode { get; set; }


        /// <summary>
        /// Customer country name
        /// </summary>
        public string Country { get; set; }

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
