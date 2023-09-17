using WMS_FE_ASP_NET_Core_Web.DTO;
using WMS_FE_ASP_NET_Core_Web.Models;

namespace WMS_FE_ASP_NET_Core_Web.Services
{
    public interface Iwrapper
    {
        public CreateCustomerModel Bind(IFormCollection customer);
        public UpdateCustomerModel BindToUpdateCustomer(IFormCollection customer);
        public RegistrationRequestModel BindToRegistrationRequest(IFormCollection customer);
        public CreateOrderModel BindToCreateOrder(IFormCollection order, int userId);
        public OrderViewModel Bind(OrderModel? order, List<OrderItemModel>? orderItems);
        public OrderViewModel Bind(OrderModel? order, List<OrderItemModel>? orderItems, List<ProductModel>? products);
        public CreateOrderViewModel Bind(List<OrderType>? orderTypes, List<CustomerModel>? customers);
        public CreateOrderItemModel Bind(int orderId, IFormCollection collection);
        public OrderItemViewModel Bind(int orderId, OrderItemModel orderItemModel);
        public UpdateOrderItemModel BindToUpdateOrderItem(IFormCollection collection);
    }
}
