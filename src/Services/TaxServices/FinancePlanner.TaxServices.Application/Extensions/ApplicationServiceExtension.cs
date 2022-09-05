using System.Reflection;
using FinancePlanner.TaxServices.Application.Behaviours;
using FluentValidation;
using MediatR;
using FinancePlanner.TaxServices.Application.PluginHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FinancePlanner.TaxServices.Infrastructure.Repositories;

namespace FinancePlanner.TaxServices.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(PluginFactory).Assembly);
            services.AddSingleton(serviceProvider =>
            {
                IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                IFederalTaxBracketRepository federalTaxBracketRepository = serviceProvider.GetRequiredService<IFederalTaxBracketRepository>();
                PluginFactory pluginFactory = new PluginFactory(configuration, federalTaxBracketRepository);
                pluginFactory.Initialize();
                return pluginFactory;
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
