using WMS.Domain.Models.DTO;

namespace WMS.Infastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUserAsync(string username);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> RegisterAsync(RegistrationRequest registrationRequest);
    }
}
