using WMS_FE_ASP_NET_Core_Web.DTO;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public interface Iwrapper
    {
        public CreateCustomerModel Bind(IFormCollection customer);
        public UpdateCustomerModel BindToUpdateCustomer(IFormCollection customer);
        public RegistrationRequestModel BindToRegistrationRequest(IFormCollection customer);
        public CreateOrderModel BindToCreateOrder(IFormCollection order, int userId);
    }
}
