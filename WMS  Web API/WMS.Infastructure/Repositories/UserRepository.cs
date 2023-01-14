using Microsoft.EntityFrameworkCore;
using System.Text;
using WMS.Domain.Models;
using WMS.Domain.Models.DTO;
using WMS.Infastructure.Database;
using WMS.Infastructure.Interfaces;

namespace WMS.Infastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WMSContext _db;
        private readonly IPasswordService _passwordService;
     

        public UserRepository(WMSContext db, IPasswordService passwordService, IJwtService jwtService)
        {
            _db = db;
            _passwordService = passwordService;
        }

        public async Task<Role?> GetRolebyNameAsync(string roleName)
        {
            var userRole = await _db.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
            return userRole;
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

        /// <summary>
        /// Login to WMS
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>WMSuser</returns>
        public async Task<WMSuser?> LoginAsync(string userName, string password)
        {
        
            var user = await _db.WMSusers.FirstOrDefaultAsync(x => x.Username.ToLower() == userName.ToLower());

            if (user == null || !_passwordService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            var userRole = await _db.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId );
            if (userRole == null) 
            { 
                return null;
            }

            user.Role= userRole;

            return user;
        }

 
        /// <summary>
        /// New user registration on WMS
        /// </summary>
        /// <param name="user"></param>
        /// <returns>bool</returns>
        public async Task<bool> RegisterAsync(WMSuser user)
        {
            _db.WMSusers.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
