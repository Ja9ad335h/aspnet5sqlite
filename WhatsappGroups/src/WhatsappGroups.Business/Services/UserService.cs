using Microsoft.AspNet.Identity;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using WhatsappGroups.Business.Core;
using WhatsappGroups.Data.Contexts;
using WhatsappGroups.Business.Models;
using Microsoft.AspNet.Authentication;

namespace WhatsappGroups.Business.Services
{
    public class UserService : IUserService
    {
        private WhatsappGroupsAdminContext _adminDb;
        public UserService(WhatsappGroupsAdminContext adminDb)
        {
            _adminDb = adminDb;
        }

        public bool Register(RegisterModel registerUser)
        {
            _adminDb.Users.Add(MapUser(registerUser));
            return _adminDb.SaveChanges() == 1;
        }

        public User ValidateOAuthUser(string userName, string password)
        {
            return _adminDb.Users.FirstOrDefault(u => u.UserName == userName && Cryptography.VerifyHash(password, u.PasswordHash, Cryptography.HashType.SHA512));
        }

        public bool ValidateOAuthClient(string clientID)
        {
            return !string.IsNullOrEmpty(clientID);
        }
        public List<string> UserRoles(int userId)
        {
            var user = _adminDb.Users.FirstOrDefault(u => u.Id == userId);
            return user.Roles.Select(r => _adminDb.Roles.FirstOrDefault(n => n.Id == r.RoleId).Name).ToList();
        }

        public User MapUser(RegisterModel model)
        {
            return new User()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BusinessName = model.BusinessName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = Cryptography.ComputeHash(model.Password, Cryptography.HashType.SHA512),
                TwoFactorEnabled = model.TwoFactorEnabled
            };
        }

        public User ValidateRefreshToken(AuthenticationTicket authenticationTicket)
        {
            throw new NotImplementedException();
        }
    }
}
