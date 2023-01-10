using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastrukture.Interfaces;

namespace WMS.Infastrukture.Services
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
    }
}
