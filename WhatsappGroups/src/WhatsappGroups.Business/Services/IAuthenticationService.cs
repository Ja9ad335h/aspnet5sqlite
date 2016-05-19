using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Services
{
    public interface IAuthenticationService
    {
        Audience AddAudience(string name);
        Audience FindAudience(string clientId);
        Task<Audience> GetAudienceAsync(string identifier, CancellationToken cancellationToken);
    }
}
