using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public class Wrapper : Iwrapper
    {
        public CreateCustomerModel Bind(IFormCollection customer)
        {
            return new CreateCustomerModel
            {
                Name = customer["Name"],
                LegalCode = customer["LegalCode"],
                Address = customer["Address"],
                City = customer["City"],
                PostCode = customer["PostCode"],
                Country = customer["Country"],
                ContactPerson = customer["ContactPerson"],
                PhoneNumber = customer["PhoneNumber"],
                Email = customer["Email"],
            };
        }


        public UpdateCustomerModel BindToUpdateCustomer(IFormCollection customer)
        {
            return new UpdateCustomerModel
            {                
                Name = customer["Name"],
                LegalCode = customer["LegalCode"],
                Address = customer["Address"],
                City = customer["City"],
                PostCode = customer["PostCode"],
                Country = customer["Country"],
                ContactPerson = customer["ContactPerson"],
                PhoneNumber = customer["PhoneNumber"],
                Email = customer["Email"],
                Status = customer["Status"][0] == "true" ? true : false
            };
        }

        public RegistrationRequestModel BindToRegistrationRequest(IFormCollection user)
        {
            return new RegistrationRequestModel
            {
                Username = user["Username"],
                Name = user["Name"],
                Password = user["Password"],
                Role = user["Role"],
            };
        }
    }
}
