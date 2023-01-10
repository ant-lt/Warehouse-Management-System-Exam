using WMS.Domain.Models;
using WMS.Domain.Models.DTO;

namespace WMS.Infastrukture.Interfaces
{
    public interface IWMSwrapper
    {
        public GetProductDto Bind(Product product);
    }
}
