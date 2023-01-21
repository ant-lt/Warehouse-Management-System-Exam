using WMS.Domain.Models;

namespace WMS.Infastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUserAsync(string username);
        Task<WMSuser?> LoginAsync(string userName, string password);
        Task<bool> RegisterAsync(WMSuser user);
        Task<Role?> GetRolebyNameAsync(string roleName);
    }
}
