using Microsoft.Extensions.WebEncoders;
using WhatsappGroups.Business.Core;
using WhatsappGroups.Data.Contexts;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Data.Entity;

namespace WhatsappGroups.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly WhatsappGroupsAdminContext _db;
        public AuthenticationService(WhatsappGroupsAdminContext db)
        {
            _db = db;
        }

        public Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncoder.ToBase64Url(key);

            Audience newAudience = new Audience() { Id = clientId, Name = name, Secret = base64Secret, IsActive = true, CreateDate = DateTime.Now };
            _db.Audiences.Add(newAudience);
            return _db.SaveChanges() == 1 ? newAudience : new Audience();
        }

        public Audience FindAudience(string id)
        {
            return _db.Audiences.FirstOrDefault(a => a.Id == id);
        }

        public Task<Audience> GetAudienceAsync(string identifier, CancellationToken cancellationToken)
        {
            return (from application in _db.Audiences
                    where application.Id == identifier
                    select application).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
