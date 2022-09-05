using FinancePlanner.TaxServices.Application.Contracts;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Models;
using FinancePlanner.TaxServices.Application.Models.Exceptions;
using Microsoft.Extensions.Configuration;
using TaxServices.Plugins.W4Before2020.Models;

namespace TaxServices.Plugins.W4Before2020
{
    public class TaxCalculatorManager : IFederalTaxWithheldCalculator
    {
        private readonly IConfiguration _configuration;

        public TaxCalculatorManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FedTaxWithheldResponse> CalculateFederalTaxWithheldAmount(CalculateFedWithheldRequest model)
        {
            int allowanceNumber= 0;
            decimal additionalAmountToWithheld = 0;

            if (model.Data.TryGetValue("AllowanceNumber", out string allowanceNumberString) &&
                int.TryParse(allowanceNumberString, out allowanceNumber))
            {
                throw new BadRequestException("Invalid AllowanceNumber passed in request");
            };

            if (model.Data.TryGetValue("AdditionalAmountToWithheld", out string additionalAmountToWithheldString) &&
                decimal.TryParse(allowanceNumberString, out additionalAmountToWithheld))
            {
                throw new BadRequestException("Invalid AdditionalAmountToWithheld passed in request");
            };

            W4Before2020Model w4Before2020Model = new W4Before2020Model
            {
                TaxableWage = model.TaxableWage,
                PayPeriodNumber = model.PayPeriodNumber,
                TaxFilingStatus = model.TaxFilingStatus,
                W4Type = model.W4Type,
                AdditionalAmountToWithheld = additionalAmountToWithheld,
                AllowanceNumber = allowanceNumber
            };

            decimal _1c = w4Before2020Model.TaxableWage * w4Before2020Model.PayPeriodNumber;
            decimal _1k = w4Before2020Model.AllowanceNumber * 4300;
            decimal _1l = _1c - _1k;
            if (_1l < 0) _1l = 0;
            decimal adjustedAnnualWage = _1l;

            // Db
            // adjustedAnnualWage at least amount of Column A but less than Column B



            FedTaxWithheldResponse response = new FedTaxWithheldResponse();
            return response;
        }

 
    }
}