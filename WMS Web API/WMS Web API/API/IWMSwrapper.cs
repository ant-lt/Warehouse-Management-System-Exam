using WMS.Domain.Models;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.API
{
    /// <summary>
    /// Maps the domain models to the DTOs
    /// </summary>
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
        public GetWarehousesRatioOfOccupiedDto Bind(Warehouse warehouse, double warehouseRatioOfOccupied);
        public GetOrderStatusDto Bind(OrderStatus orderStatus);
        public GetOrderTypesDto Bind(OrderType orderType);
    }
}
