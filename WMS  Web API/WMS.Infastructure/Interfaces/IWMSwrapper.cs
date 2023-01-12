using WMS.Domain.Models;
using WMS.Domain.Models.DTO;

namespace WMS.Infastructure.Interfaces
{
    public interface IWMSwrapper
    {
        public GetProductDto Bind(Product product);
        public GetCustomerDto Bind(Customer customer);
        public GetOrderDto Bind(Order order);
        public GetShipmentDto Bind(Shipment shipment);
        public GetInventoryDto Bind(Inventory inventory);
        public Customer Bind(CreateCustomerDto customerDto);
        public Customer Bind(UpdateCustomerDto updateCustomerDto, Customer customer);
        public Order Bind(CreateOrderDto req);
        public Order Bind(UpdateOrderDto updateOrderDto, Order order);
        public GetOrderItemDto Bind(OrderItem orderItem);
        public GetShipmentItemDto Bind(ShipmentItem shipmentItem);
        public OrderItem Bind(CreateOrderItemDto orderItem);
        public OrderItem Bind(UpdateOrderItemDto updateOrderItemDto, OrderItem orderItem);
    }
}
