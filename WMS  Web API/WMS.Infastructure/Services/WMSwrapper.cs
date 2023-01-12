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
                OrderStatusId = order.OrderStatusId,
                OrderTypeId = order.OrderTypeId,
                CustomerId = order.CustomerId,
                WMSuserId = order.WMSuserId
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
                ShipmentStatusId = shipment.ShipmentStatusId,
                OrderId = shipment.OrderId,
                CustomerId = shipment.CustomerId,
                WMSuserId = shipment.WMSuserId
            };
        }

        public GetInventoryDto Bind(Inventory inventory)
        {
            return new GetInventoryDto
            {
                Id = inventory.Id,
                Quantity = inventory.Quantity,
                WarehouseId = inventory.WarehouseId,
                ProductId = inventory.ProductId
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
                CustomerId = req.OrderTypeId,
                WMSuserId = req.WMSuserId
            };
        }

        public Order Bind(UpdateOrderDto updateOrderDto, Order order)
        {
            order.ScheduledDate = updateOrderDto.ScheduledDate;
            order.ExecutionDate = updateOrderDto.ExecutionDate;
            order.OrderStatusId = updateOrderDto.OrderStatusId;
            return order;
        }

        public GetOrderItemDto Bind(OrderItem orderItem)
        {
            return new GetOrderItemDto
            {
                Id= orderItem.Id,
                OrderId = orderItem.OrderId,
                Quantity= orderItem.Quantity,
                ProductId= orderItem.ProductId
            };
        }
    }
}
