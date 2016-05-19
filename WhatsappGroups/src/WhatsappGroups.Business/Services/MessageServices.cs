using Microsoft.AspNet.Identity;
using WhatsappGroups.Data.Contexts;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly WhatsappGroupsAdminContext _db;
        public AuthMessageSender(WhatsappGroupsAdminContext db)
        {
            _db = db;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        public Task Register(string username, string password, string email)
        {

            var user = new User { UserName = username, Email = email, PasswordHash = password };

            _db.Users.Add(user);
            return _db.SaveChangesAsync();

        }
    }
}
