using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
        Task Register(string username, string password, string email);
    }
}
