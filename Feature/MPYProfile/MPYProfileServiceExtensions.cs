using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public static class MPYProfileServiceExtensions
    {
        public static IServiceCollection AddMPYProfileService(this IServiceCollection service)
        {
            service.AddSingleton<IMPYProfileRepository, MPYProfileRepository>();
            service.AddSingleton<IMPYProfileService, MPYProfileService>();

            return service;
        }
    }
}
