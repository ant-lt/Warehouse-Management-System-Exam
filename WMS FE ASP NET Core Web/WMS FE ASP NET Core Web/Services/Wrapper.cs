using WMS_FE_ASP_NET_Core_Web.DTO;
using WMS_FE_ASP_NET_Core_Web.Models;

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

        public CreateOrderModel BindToCreateOrder(IFormCollection order, int userId)
        {
            return new CreateOrderModel
            {
                ScheduledDate = DateTime.Parse(order["ScheduledDate"]),
                ExecutionDate = DateTime.Now,
                OrderStatusId = 1,
                OrderTypeId = int.Parse(order["OrderTypeId"]),
                CustomerId = int.Parse(order["CustomerId"]),
                WMSuserId = userId,
            };
        }

        public OrderViewModel Bind(OrderModel? order, List<OrderItemModel>? orderItems)
        {
            double totalVolume = 0;

            if (orderItems != null)
            {
                foreach (var item in orderItems!)
                {
                    totalVolume += item.Volume;
                }
            }

            var orderViewModel = new OrderViewModel
            {
                Order = order ?? new OrderModel(),
                OrderItems = orderItems ?? new List<OrderItemModel>(),
                TotalVolume = totalVolume
            };
            return orderViewModel;
        }

        public CreateOrderViewModel Bind(List<OrderType>? orderTypes, List<CustomerModel>? customers)
        {
            return new CreateOrderViewModel
            {
                OrderTypes = orderTypes,
                Customers = customers
            };
        }

        public CreateOrderItemModel Bind(int orderId, IFormCollection collection)
        {
            return new CreateOrderItemModel
            {
                OrderId = orderId,
                ProductId = int.Parse(collection["ProductId"]),
                Quantity = int.Parse(collection["Quantity"]),
            };
        }

        public OrderViewModel Bind(OrderModel? order, List<OrderItemModel>? orderItems, List<ProductModel>? products)
        {
            double totalVolume = 0;
            if (orderItems != null)
            {
                foreach (var item in orderItems!)
                {
                    totalVolume += item.Volume;
                }
            }

            return new OrderViewModel
            {
                Order = order ?? new OrderModel(),
                OrderItems = orderItems ?? new List<OrderItemModel>(),
                TotalVolume = totalVolume,
                Products = products ?? new List<ProductModel>()
            };
        }

        public OrderItemViewModel Bind(int orderId, OrderItemModel orderItemModel)
        {
            return new OrderItemViewModel
            {
                Id = orderItemModel.Id,
                OrderId = orderId,
                ProductSKU = orderItemModel.ProductSKU,
                ProductName = orderItemModel.ProductName,
                ProductDescription = orderItemModel.ProductDescription,
                Volume = orderItemModel.Volume,
                Quantity = orderItemModel.Quantity,
                ProductId = orderItemModel.ProductId
            };
        }

        public UpdateOrderItemModel BindToUpdateOrderItem(IFormCollection collection)
        {
            return new UpdateOrderItemModel
            {
                ProductId = int.Parse(collection["ProductId"]),
                Quantity = decimal.Parse(collection["Quantity"]),
            };
        }
    }
}
