using FinancePlanner.TaxServices.Application.Contracts;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions;
using FinancePlanner.TaxServices.Application.Models;
using FinancePlanner.TaxServices.Domain.Entities;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using TaxServices.Plugins.W4Before2020.Models;

namespace TaxServices.Plugins.W4Before2020
{
    public class TaxCalculatorProcessManager : IFederalTaxWithheldCalculator
    {
        private readonly IConfiguration _configuration;
        private readonly TaxCalculatorManager _taxCalculatorManager;
        private readonly IFederalTaxBracketRepository _federalTaxBracketRepository;

        public TaxCalculatorProcessManager(IConfiguration configuration, IFederalTaxBracketRepository federalTaxBracketRepository)
        {
            _configuration = configuration;
            _federalTaxBracketRepository = federalTaxBracketRepository;
            _taxCalculatorManager = new TaxCalculatorManager(_federalTaxBracketRepository);
        }

        public async Task<FedTaxWithheldResponse> CalculateFederalTaxWithheldAmount(CalculateFedWithheldRequest model)
        {
            W4Before2020Model w4Before2020Model = _taxCalculatorManager.GetModel(model);
            decimal adjustedAnnualWage = _taxCalculatorManager.GetAdjustedAnnualWage(w4Before2020Model);
            decimal federalTaxWithheldAmount = await _taxCalculatorManager.GetFederalTaxWithheldAmount(w4Before2020Model, adjustedAnnualWage);
            var a = _federalTaxBracketRepository;

            PercentageMethodTable percentageMethodTable = await _federalTaxBracketRepository.GetFederalTaxPercentage(adjustedAnnualWage, "tableName");

            FedTaxWithheldResponse response = new FedTaxWithheldResponse
            {
                TaxableWage = model.TaxableWage,
                FederalTaxWithheldAmount = federalTaxWithheldAmount
            };

            return response;
        }

        public async Task<TaxDeductionsResponse> CalculateTaxDeductions(CalculateTaxDeductionsRequest model)
        {
            throw new System.NotImplementedException();
        }
    }
}