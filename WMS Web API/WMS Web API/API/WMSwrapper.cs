using WMS.Domain.Models;
using WMS_Web_API.API.DTO;

namespace WMS_Web_API.API
{
    /// <summary>
    /// Maps the domain models to the DTOs
    /// </summary>
    public class WMSwrapper : IWMSwrapper
    {
        /// <summary>
        /// Bind a <see cref="Product"/> object to a <see cref="GetProductDto"/> object.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> object to bind.</param>
        /// <returns>The bound <see cref="GetProductDto"/> object.</returns>
        public GetProductDto Bind(Product product)
        {
            return new GetProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                Volume = product.Volume,
                Weigth = product.Weigth
            };
        }

        /// <summary>
        /// Bind a <see cref="Customer"/> object to a <see cref="GetCustomerDto"/> object.
        /// </summary>
        /// <param name="customer">The <see cref="Customer"/> object to bind.</param>
        /// <returns>The bound <see cref="GetCustomerDto"/> object.</returns>
        public GetCustomerDto Bind(Customer customer)
        {
            return new GetCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                LegalCode = customer.LegalCode,
                Address = customer.Address,
                City = customer.City,
                PostCode = customer.PostCode,
                Country = customer.Country,
                ContactPerson = customer.ContactPerson,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Status = customer.Status,
                Created = customer.Created
            };
        }

        /// <summary>
        /// Bind a <see cref="Order"/> object to a <see cref="GetOrderDto"/> object.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> object to bind.</param>
        /// <returns>The bound <see cref="GetOrderDto"/> object.</returns>
        public GetOrderDto Bind(Order order)
        {
            return new GetOrderDto
            {
                Id = order.Id,
                Date = order.Date,
                ScheduledDate = order.ScheduledDate,
                ExecutionDate = order.ExecutionDate,
                OrderStatus = order.OrderStatus.Name,
                OrderType = order.OrderType.Name,
                CustomerName = order.Customer.Name,
                CreatedByUser = order.RWMSuser.Name
            };
        }

        /// <summary>
        /// Bind a <see cref="Shipment"/> object to a <see cref="GetShipmentDto"/> object.
        /// </summary>
        /// <param name="shipment">The <see cref="Shipment"/> object to bind.</param>
        /// <returns>The bound <see cref="GetShipmentDto"/> object.</returns>
        public GetShipmentDto Bind(Shipment shipment)
        {
            return new GetShipmentDto
            {
                Id = shipment.Id,
                Date = shipment.Date,
                ScheduledDate = shipment.ScheduledDate,
                ExecutionDate = shipment.ExecutionDate,
                ShipmentStatus = shipment.ShipmentStatus.Name,
                OrderId = shipment.OrderId,
                CustomerName = shipment.Customer.Name,
                UserName = shipment.RWMSuser.Name
            };
        }

        /// <summary>
        /// Bind a <see cref="Inventory"/> object to a <see cref="GetInventoryDto"/> object.
        /// </summary>
        /// <param name="inventory">The <see cref="Inventory"/> object to bind.</param>
        /// <returns>The bound <see cref="GetInventoryDto"/> object.</returns>
        public GetInventoryDto Bind(Inventory inventory)
        {
            return new GetInventoryDto
            {
                Id = inventory.Id,
                Quantity = inventory.Quantity,
                WarehouseName = inventory.Warehouse.Name,
                ProductName = inventory.Product.Name,
                ProductSKU = inventory.Product.SKU,
                ProductDescription = inventory.Product.Description
            };
        }

        /// <summary>
        /// Bind a <see cref="CreateCustomerDto"/> object to a <see cref="Customer"/> object.
        /// </summary>
        /// <param name="customerDto">The <see cref="CreateCustomerDto"/> object to bind.</param>
        /// <returns></returns>
        public Customer Bind(CreateCustomerDto customerDto)
        {
            return new Customer
            {
                Name = customerDto.Name,
                LegalCode = customerDto.LegalCode,
                Address = customerDto.Address,
                City = customerDto.City,
                PostCode = customerDto.PostCode,
                Country = customerDto.Country,
                ContactPerson = customerDto.ContactPerson,
                PhoneNumber = customerDto.PhoneNumber,
                Email = customerDto.Email
            };
        }

        /// <summary>
        /// Bind an <see cref="UpdateCustomerDto"/> object to a <see cref="Customer"/> object.
        /// </summary>
        /// <param name="updateCustomerDto">The <see cref="UpdateCustomerDto"/> object to bind.</param>
        /// <param name="customer">The <see cref="Customer"/> object to bind.</param>
        /// <returns>The bound <see cref="Customer"/> object.</returns>
        public Customer Bind(UpdateCustomerDto updateCustomerDto, Customer customer)
        {
            customer.Name = updateCustomerDto.Name;
            customer.LegalCode = updateCustomerDto.LegalCode;
            customer.Address = updateCustomerDto.Address;
            customer.City = updateCustomerDto.City;
            customer.PostCode = updateCustomerDto.PostCode;
            customer.Country = updateCustomerDto.Country;
            customer.ContactPerson = updateCustomerDto.ContactPerson;
            customer.PhoneNumber = updateCustomerDto.PhoneNumber;
            customer.Email = updateCustomerDto.Email;
            customer.Status = updateCustomerDto.Status;
            return customer;
        }

        /// <summary>
        /// Bind a <see cref="CreateOrderDto"/> object to a <see cref="Order"/> object.
        /// </summary>
        /// <param name="req">The <see cref="CreateOrderDto"/> object to bind.</param>
        /// <returns>The bound <see cref="Order"/> object.</returns>
        public Order Bind(CreateOrderDto req)
        {
            return new Order
            {
                ScheduledDate = req.ScheduledDate,
                ExecutionDate = req.ExecutionDate,
                OrderStatusId = req.OrderStatusId,
                OrderTypeId = req.OrderTypeId,
                CustomerId = req.CustomerId,
                WMSuserId = req.WMSuserId
            };
        }

        /// <summary>
        /// Bind an <see cref="UpdateOrderDto"/> object to a <see cref="Order"/> object.
        /// </summary>
        /// <param name="updateOrderDto">The <see cref="UpdateOrderDto"/> object to bind.</param>
        /// <param name="order">The <see cref="Order"/> object to bind.</param>
        /// <returns>The bound <see cref="Order"/> object.</returns>
        public Order Bind(UpdateOrderDto updateOrderDto, Order order)
        {
            order.ScheduledDate = updateOrderDto.ScheduledDate;
            order.ExecutionDate = updateOrderDto.ExecutionDate;
            order.OrderStatusId = updateOrderDto.OrderStatusId;
            order.OrderTypeId = updateOrderDto.OrderTypeId;
            return order;
        }

        /// <summary>
        /// Bind a <see cref="OrderItem"/> object to a <see cref="GetOrderItemDto"/> object.
        /// </summary>
        /// <param name="orderItem">The <see cref="OrderItem"/> object to bind.</param>
        /// <returns>The bound <see cref="GetOrderItemDto"/> object.</returns>
        public GetOrderItemDto Bind(OrderItem orderItem)
        {
            return new GetOrderItemDto
            {
                Id= orderItem.Id, 
                Quantity= orderItem.Quantity,
                ProductSKU = orderItem.Product.SKU,
                ProductName = orderItem.Product.Name,
                ProductDescription = orderItem.Product.Description,
                Volume = orderItem.Quantity * orderItem.Product.Volume,
                ProductId = orderItem.ProductId
            };
        }

        /// <summary>
        /// Bind a <see cref="ShipmentItem"/> object to a <see cref="GetShipmentItemDto"/> object.
        /// </summary>
        /// <param name="shipmentItem">The <see cref="ShipmentItem"/> object to bind.</param>
        /// <returns>The bound <see cref="GetShipmentItemDto"/> object.</returns>
        public GetShipmentItemDto Bind(ShipmentItem shipmentItem)
        {
            return new GetShipmentItemDto
            {
                Id = shipmentItem.Id,
                Quantity = shipmentItem.Quantity,
                ProductSKU = shipmentItem.Product.SKU,
                ProductName = shipmentItem.Product.Name,
                ProductDescription = shipmentItem.Product.Description
            };
        }

        /// <summary>
        /// Bind a <see cref="CreateOrderItemDto"/> object to a <see cref="OrderItem"/> object.
        /// </summary>
        /// <param name="orderItem">The <see cref="CreateOrderItemDto"/> object to bind.</param>
        /// <returns>The bound <see cref="OrderItem"/> object.</returns>
        public OrderItem Bind(CreateOrderItemDto orderItem)
        {
            return new OrderItem
            {
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId
            };
        }

        /// <summary>
        /// Bind a <see cref="UpdateOrderItemDto"/> object to a <see cref="OrderItem"/> object.
        /// </summary>
        /// <param name="updateOrderItemDto">The <see cref="UpdateOrderItemDto"/> object to bind.</param>
        /// <param name="orderItem">The <see cref="OrderItem"/> object to bind.</param>
        /// <returns>The bound <see cref="OrderItem"/> object.</returns>
        public OrderItem Bind(UpdateOrderItemDto updateOrderItemDto, OrderItem orderItem)
        {
            orderItem.Quantity = updateOrderItemDto.Quantity;
            orderItem.ProductId = updateOrderItemDto.ProductId;
            return orderItem;
        }

        /// <summary>
        /// Bind a <see cref="Warehouse"/> object to a <see cref="GetWarehousesRatioOfOccupiedDto"/> object.
        /// </summary>
        /// <param name="warehouse">The <see cref="Warehouse"/> object to bind.</param>
        /// <param name="warehouseRatioOfOccupied">The <see cref="double"/> object to bind.</param>
        /// <returns>The bound <see cref="GetWarehousesRatioOfOccupiedDto"/> object.</returns>
        public GetWarehousesRatioOfOccupiedDto Bind(Warehouse warehouse, double warehouseRatioOfOccupied)
        {
            return new GetWarehousesRatioOfOccupiedDto
            {
                Id = warehouse.Id,
                WarehouseName = warehouse.Name,
                WarehouseDescription = warehouse.Description,
                WarehouseLocation = warehouse.Location,
                WarehouseTotalVolumeCapacity = (double)warehouse.TotalVolume,
                WarehouseActualTotalOccupiedVolume = warehouseRatioOfOccupied,
                WarehouseAvailableTotalVolume = (double)warehouse.TotalVolume - (double)warehouseRatioOfOccupied
            };
        }

        /// <summary>
        /// Bind a <see cref="OrderStatus"/> object to a <see cref="GetOrderStatusDto"/> object.
        /// </summary>
        /// <param name="orderStatus">The <see cref="OrderStatus"/> object to bind.</param>
        /// <returns>The bound <see cref="GetOrderStatusDto"/> object.</returns>
        public GetOrderStatusDto Bind(OrderStatus orderStatus)
        {
            return new GetOrderStatusDto
            {
                Id = orderStatus.Id,
                OrderStatusName = orderStatus.Name
            };
        }

        /// <summary>
        /// Bind a <see cref="OrderType"/> object to a <see cref="GetOrderTypesDto"/> object.
        /// </summary>
        /// <param name="orderType">The <see cref="OrderType"/> object to bind.</param>
        /// <returns>The bound <see cref="GetOrderTypesDto"/> object.</returns>
        public GetOrderTypesDto Bind(OrderType orderType)
        {
            return new GetOrderTypesDto
            {
                Id = orderType.Id,
                OrderTypeName = orderType.Name,
            };
        }
    }
}
