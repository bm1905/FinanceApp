using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Contracts;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions;
using FinancePlanner.TaxServices.Application.Models;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using TaxServices.Plugins.W4FromOrAfter2020.Models;

namespace TaxServices.Plugins.W4FromOrAfter2020
{
    public class TaxCalculatorProcessManager : IFederalTaxWithheldCalculator
    {
        private readonly IConfiguration _configuration;
        private readonly TaxCalculatorManager _taxCalculatorManager;

        public TaxCalculatorProcessManager(IConfiguration configuration, IFederalTaxBracketRepository federalTaxBracketRepository)
        {
            _configuration = configuration;
            _taxCalculatorManager = new TaxCalculatorManager(federalTaxBracketRepository);
        }

        public async Task<FedTaxWithheldResponse> CalculateFederalTaxWithheldAmount(CalculateFedWithheldRequest model)
        {
            W4FromOrAfter2020Model w4FromOrAfter2020Model = _taxCalculatorManager.GetModel(model);
            decimal adjustedAnnualWage = _taxCalculatorManager.GetAdjustedAnnualWage(w4FromOrAfter2020Model);
            decimal federalTaxWithheldAmount = await _taxCalculatorManager.GetFederalTaxWithheldAmount(w4FromOrAfter2020Model, adjustedAnnualWage);

            FedTaxWithheldResponse response = new FedTaxWithheldResponse
            {
                TaxableWage = model.TaxableWage,
                FederalTaxWithheldAmount = federalTaxWithheldAmount
            };

            return response;
        }

        public async Task<TaxDeductionsResponse> CalculateTaxDeductions(CalculateTaxDeductionsRequest model)
        {
            W4FromOrAfter2020Model w4FromOrAfter2020Model = _taxCalculatorManager.GetModel(model);
            decimal adjustedAnnualWage = _taxCalculatorManager.GetAdjustedAnnualWage(w4FromOrAfter2020Model);
            decimal federalTaxWithheldAmount = await _taxCalculatorManager.GetFederalTaxWithheldAmount(w4FromOrAfter2020Model, adjustedAnnualWage);

            throw new System.NotImplementedException();
        }
    }
}