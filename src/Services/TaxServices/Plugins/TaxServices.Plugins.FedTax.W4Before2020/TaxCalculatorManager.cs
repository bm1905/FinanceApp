using System;
using System.Threading.Tasks;
using FinancePlanner.Shared.Models.Enums;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.TaxServices;
using FinancePlanner.TaxServices.Application.Constants;
using FinancePlanner.TaxServices.Domain.Entities;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using TaxServices.Plugins.FedTax.W4Before2020.Models;

namespace TaxServices.Plugins.FedTax.W4Before2020
{
    public class TaxCalculatorManager
    {
        private readonly IFederalTaxRepository _federalTaxBracketRepository;

        public TaxCalculatorManager(IFederalTaxRepository federalTaxBracketRepository)
        {
            _federalTaxBracketRepository = federalTaxBracketRepository;
        }

        internal W4Before2020Model GetModel(CalculateTaxWithheldRequest model)
        {
            model.Data.TryGetValue("AllowanceNumber", out string allowanceNumberString);
            if (string.IsNullOrEmpty(allowanceNumberString))
            {
                throw new BadRequestException("Could not find AllowanceNumber in request");
            }

            if (!int.TryParse(allowanceNumberString, out int allowanceNumber))
            {
                throw new BadRequestException("Invalid AllowanceNumber passed in request");
            }

            model.Data.TryGetValue("AdditionalAmountToWithheld", out string additionalAmountToWithheldString);
            if (string.IsNullOrEmpty(additionalAmountToWithheldString))
            {
                throw new BadRequestException("Could not find AdditionalAmountToWithheld in request");
            }

            if (!decimal.TryParse(additionalAmountToWithheldString, out decimal additionalAmountToWithheld))
            {
                throw new BadRequestException("Invalid AdditionalAmountToWithheld passed in request");
            }

            W4Before2020Model w4Before2020Model = new W4Before2020Model
            {
                TaxableWage = model.FederalTaxableWage,
                PayPeriodNumber = model.PayPeriodNumber,
                TaxFilingStatus = model.TaxFilingStatus,
                W4Type = model.W4Type,
                AdditionalAmountToWithheld = additionalAmountToWithheld,
                AllowanceNumber = allowanceNumber
            };

            return w4Before2020Model;
        }

        internal decimal GetAdjustedAnnualWage(W4Before2020Model w4Before2020Model, IConfiguration config)
        {
            try
            {
                decimal _1c = w4Before2020Model.TaxableWage * w4Before2020Model.PayPeriodNumber;
                decimal _1k = w4Before2020Model.AllowanceNumber * int.Parse(config.GetSection("W4Config:W4Before2020:Allowance_1k").Value ?? "4300");
                decimal _1l = _1c - _1k;
                if (_1l < 0) _1l = 0;
                return _1l;
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Something went wrong while calculating Adjusted Annual Wage.", ex);
            }
        }

        internal async Task<decimal> GetFederalTaxWithheldAmount(W4Before2020Model w4Before2020Model, decimal adjustedAnnualWage)
        {
            try
            {
                string tableName = w4Before2020Model.TaxFilingStatus switch
                {
                    TaxFilingStatus.MarriedFilingJointly => TaxMethodTables.MarriedFiledJointlyW4Before2020,
                    TaxFilingStatus.SingleOrMarriedFilingSingle => TaxMethodTables.SingleOrMarriedFiledSeparatelyW4Before2020,
                    TaxFilingStatus.HeadOfHousehold => TaxMethodTables.HeadOfHouseholdW4Before2020,
                    _ => string.Empty,
                };
                PercentageMethodTable percentageMethodTable = await _federalTaxBracketRepository.GetFederalTaxPercentage(adjustedAnnualWage, tableName);

                decimal _2b = percentageMethodTable.AtLeast;
                decimal _2c = percentageMethodTable.TentativeHoldAmount;
                decimal _2d = percentageMethodTable.Percentage;
                decimal _2e = adjustedAnnualWage - _2b;
                decimal _2f = _2e * _2d / 100;
                decimal _2g = _2c + _2f;
                decimal _2h = _2g / w4Before2020Model.PayPeriodNumber;
                decimal _4b = w4Before2020Model.AdditionalAmountToWithheld + _2h;
                return _4b;
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"Something went wrong while calculating Federal Tax Withheld Amount. {ex.Message}", ex);
            }
        }
    }
}
