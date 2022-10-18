using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;

namespace FinancePlanner.API.Gateway.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureOcelot();
            services.ConfigureAuthentication(config);
            return services;
        }

        // Ocelot Configuration
        private static void ConfigureOcelot(this IServiceCollection services)
        {
            services.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                .AddConsul();
        }

        // Authentication
        private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            //const string authenticationProviderKey = "IdentityApiKey";
            //services.AddAuthentication()
            //    .AddJwtBearer(authenticationProviderKey, x =>
            //    {
            //        x.Authority = config["IdentityServer:BaseUrl"]; // IDENTITY SERVER URL
            //        //x.RequireHttpsMetadata = false;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };
            //    });
        }
    }
}
