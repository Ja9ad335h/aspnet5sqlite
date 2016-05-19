using WhatsappGroups.Business.Models;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;

namespace WhatsappGroups.Business.Services
{
    public interface IUserService
    {
        User ValidateOAuthUser(string userName, string password);
        bool ValidateOAuthClient(string clientID);
        bool Register(RegisterModel registerUser);
        List<string> UserRoles(int userId);
        User ValidateRefreshToken(AuthenticationTicket authenticationTicket);
    }
}
