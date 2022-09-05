using MediatR;
using FinancePlanner.TaxServices.Application.PluginHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.TaxServices.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(PluginFactory).Assembly);
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var pluginFactory = new PluginFactory(configuration);
                pluginFactory.Initialize();
                return pluginFactory;
            });
            return services;
        }
    }
}
