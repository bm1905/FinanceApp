using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFederalTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;
using FinancePlanner.TaxServices.Application.Services.FederalTaxServices;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using TaxServices.Plugins.FedTax.W4Before2020.Models;

namespace TaxServices.Plugins.FedTax.W4Before2020
{
    public class TaxCalculatorProcessManager : IFederalTaxServices
    {
        public IConfiguration Configuration { get; }
        private readonly TaxCalculatorManager _taxCalculatorManager;

        public TaxCalculatorProcessManager(IConfiguration configuration, IFederalTaxRepository federalTaxBracketRepository)
        {
            Configuration = configuration;
            _taxCalculatorManager = new TaxCalculatorManager(federalTaxBracketRepository);
        }

        public async Task<GetFederalTaxWithheldQueryResponse> CalculateFederalTaxWithheldAmount(CalculateTaxWithheldRequest model)
        {
            W4Before2020Model w4Before2020Model = _taxCalculatorManager.GetModel(model);
            decimal adjustedAnnualWage = _taxCalculatorManager.GetAdjustedAnnualWage(w4Before2020Model);
            decimal federalTaxWithheldAmount = await _taxCalculatorManager.GetFederalTaxWithheldAmount(w4Before2020Model, adjustedAnnualWage);

            GetFederalTaxWithheldQueryResponse response = new GetFederalTaxWithheldQueryResponse
            {
                FederalTaxableWage = model.FederalTaxableWage,
                FederalTaxWithheldAmount = federalTaxWithheldAmount
            };

            return response;
        }
    }
}