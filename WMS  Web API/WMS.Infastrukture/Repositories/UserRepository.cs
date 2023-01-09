using Microsoft.EntityFrameworkCore;
using System.Text;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastrukture.Database;
using WMS.Infastrukture.Interfaces;

namespace WMS.Infastrukture.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WMSContext _db;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public UserRepository(WMSContext db, IPasswordService passwordService, IJwtService jwtService)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Should return a flag indicating if a user with a specified username already exists
        /// </summary>
        /// <param name="username">Registration username</param>
        /// <returns>A flag indicating if username already exists</returns>
        public async Task<bool> IsUniqueUserAsync(string username)
        {
            var user = await _db.WMSusers.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var inputPasswordBytes = Encoding.UTF8.GetBytes(loginRequest.Password);
            var user = await _db.WMSusers.FirstOrDefaultAsync(x => x.Username.ToLower() == loginRequest.Username.ToLower());


            if (user == null || !_passwordService.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResponse
                {
                    UserName = "",
                    Token = "",        
                    Active= false,
                };
            }

            var userRole = await _db.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
            var userRoleName = userRole.Name;

            //var token = _jwtService.GetJwtToken(user.Id, user.RoleName);
            var token = _jwtService.GetJwtToken(user.Id, userRoleName);

            user.Role = null;

            LoginResponse loginResponse = new()
            {
                UserName = user.Username,
                Active= user.Active,
                Token = token,
            };

            return loginResponse;
        }

        // Add RegistrationResponse (Should not include password)
        // Add adapter classes to map to wanted classes
        public async Task<bool> RegisterAsync(RegistrationRequest registrationRequest)
        {

            _passwordService.CreatePasswordHash(registrationRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userRole = await _db.Roles.FirstOrDefaultAsync(x => x.Name == registrationRequest.Role);
           
            WMSuser user = new()
            {
                Username = registrationRequest.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = registrationRequest.Name,
                Role = userRole,
                Active = true,

            };

            _db.WMSusers.Add(user);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
