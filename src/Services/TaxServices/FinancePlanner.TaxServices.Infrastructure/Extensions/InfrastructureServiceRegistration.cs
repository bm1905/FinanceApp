using FinancePlanner.TaxServices.Infrastructure.Persistence;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancePlanner.TaxServices.Infrastructure.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IDapperContext, DapperContext>();
            services.AddSingleton<IFederalTaxBracketRepository, FederalTaxBracketRepository>();
            services.AddSingleton<IFederalTaxBracketRepository>(b => new FederalTaxBracketRepository(b.GetRequiredService<IDapperContext>()));
            return services;
        }
    }
}
