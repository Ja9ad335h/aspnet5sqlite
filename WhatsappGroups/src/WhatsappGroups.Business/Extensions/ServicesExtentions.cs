using Microsoft.Extensions.DependencyInjection;
using WhatsappGroups.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Extensions
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddScopedDbContexts(this IServiceCollection services)
        {
            return services
                .AddScoped<WhatsappGroupsAdminContext>()
                .AddScoped<WhatsappGroupsDataContext>();
        }
    }
}
