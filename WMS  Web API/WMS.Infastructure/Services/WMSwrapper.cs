using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Services
{
    public class WMSwrapper : IWMSwrapper
    {
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

        public Order Bind(UpdateOrderDto updateOrderDto, Order order)
        {
            order.ScheduledDate = updateOrderDto.ScheduledDate;
            order.ExecutionDate = updateOrderDto.ExecutionDate;
            order.OrderStatusId = updateOrderDto.OrderStatusId;
            order.OrderTypeId = updateOrderDto.OrderTypeId;
            return order;
        }

        public GetOrderItemDto Bind(OrderItem orderItem)
        {
            return new GetOrderItemDto
            {
                Id= orderItem.Id, 
                Quantity= orderItem.Quantity,
                ProductSKU = orderItem.Product.SKU,
                ProductName = orderItem.Product.Name,
                ProductDescription = orderItem.Product.Description
            };
        }

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

        public OrderItem Bind(CreateOrderItemDto orderItem)
        {
            return new OrderItem
            {
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId
            };
        }

        public OrderItem Bind(UpdateOrderItemDto updateOrderItemDto, OrderItem orderItem)
        {
            orderItem.Quantity = updateOrderItemDto.Quantity;
            orderItem.ProductId = updateOrderItemDto.ProductId;
            return orderItem;
        }

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

        public GetOrderStatusDto Bind(OrderStatus orderStatus)
        {
            return new GetOrderStatusDto
            {
                Id = orderStatus.Id,
                OrderStatusName = orderStatus.Name
            };
        }

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
