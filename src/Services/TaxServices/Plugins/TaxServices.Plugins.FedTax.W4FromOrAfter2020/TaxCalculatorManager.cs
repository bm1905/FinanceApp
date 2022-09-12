using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Constants;
using FinancePlanner.TaxServices.Application.Enums;
using FinancePlanner.TaxServices.Application.Models;
using FinancePlanner.TaxServices.Application.Models.Exceptions;
using FinancePlanner.TaxServices.Domain.Entities;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using TaxServices.Plugins.FedTax.W4FromOrAfter2020.Models;

namespace TaxServices.Plugins.FedTax.W4FromOrAfter2020
{
    public class TaxCalculatorManager
    {
        private readonly IFederalTaxRepository _federalTaxBracketRepository;

        public TaxCalculatorManager(IFederalTaxRepository federalTaxBracketRepository)
        {
            _federalTaxBracketRepository = federalTaxBracketRepository;
        }

        internal W4FromOrAfter2020Model GetModel(CalculateTaxWithheldRequest model)
        {
            model.Data.TryGetValue("IsMultipleJobsChecked", out string multipleJobsString);
            if (string.IsNullOrEmpty(multipleJobsString))
            {
                throw new BadRequestException("Could not find IsMultipleJobsChecked in request");
            }

            if (!bool.TryParse(multipleJobsString, out bool multipleJobs))
            {
                throw new BadRequestException("Invalid IsMultipleJobsChecked passed in request");
            }

            model.Data.TryGetValue("ExtraWithholding", out string extraWithholdingString);
            if (string.IsNullOrEmpty(extraWithholdingString))
            {
                throw new BadRequestException("Could not find ExtraWithholding in request");
            }

            if (!decimal.TryParse(extraWithholdingString, out decimal extraWithholding))
            {
                throw new BadRequestException("Invalid ExtraWithholding passed in request");
            }

            model.Data.TryGetValue("Deductions", out string deductionsString);
            if (string.IsNullOrEmpty(deductionsString))
            {
                throw new BadRequestException("Could not find Deductions in request");
            }

            if (!decimal.TryParse(deductionsString, out decimal deductions))
            {
                throw new BadRequestException("Invalid Deductions passed in request");
            }

            model.Data.TryGetValue("OtherIncome", out string otherIncomeString);
            if (string.IsNullOrEmpty(otherIncomeString))
            {
                throw new BadRequestException("Could not find OtherIncome in request");
            }

            if (!decimal.TryParse(otherIncomeString, out decimal otherIncome))
            {
                throw new BadRequestException("Invalid OtherIncome passed in request");
            }

            model.Data.TryGetValue("ClaimDependentsAmount", out string claimDependentsAmountString);
            if (string.IsNullOrEmpty(claimDependentsAmountString))
            {
                throw new BadRequestException("Could not find ClaimDependentsAmount in request");
            }

            if (!decimal.TryParse(claimDependentsAmountString, out decimal claimDependentsAmount))
            {
                throw new BadRequestException("Invalid ClaimDependentsAmount passed in request");
            }

            W4FromOrAfter2020Model w4FromOrAfter2020Model = new W4FromOrAfter2020Model
            {
                TaxableWage = model.FederalTaxableWage,
                PayPeriodNumber = model.PayPeriodNumber,
                TaxFilingStatus = model.TaxFilingStatus,
                W4Type = model.W4Type,
                ClaimDependentsAmount = claimDependentsAmount,
                Deductions = deductions,
                ExtraWithholding = extraWithholding,
                IsMultipleJobsChecked = multipleJobs,
                OtherIncome = otherIncome
            };

            return w4FromOrAfter2020Model;
        }

        internal decimal GetAdjustedAnnualWage(W4FromOrAfter2020Model w4FromOrAfter2020Model)
        {
            decimal _1c = w4FromOrAfter2020Model.TaxableWage * w4FromOrAfter2020Model.PayPeriodNumber;
            decimal _1d = w4FromOrAfter2020Model.OtherIncome;
            decimal _1e = _1c + _1d;
            decimal _1f = w4FromOrAfter2020Model.Deductions;
            decimal _1g;
            if (w4FromOrAfter2020Model.IsMultipleJobsChecked)
            {
                _1g = 0;
            }
            else if (w4FromOrAfter2020Model.TaxFilingStatus == TaxFilingStatus.MarriedFilingJointly)
            {
                _1g = 12900;
            }
            else
            {
                _1g = 8600;
            }
            decimal _1h = _1f + _1g;
            decimal _1i = _1e - _1h;
            if (_1i < 0) _1i = 0;
            return _1i;
        }

        internal async Task<decimal> GetFederalTaxWithheldAmount(W4FromOrAfter2020Model w4FromOrAfter2020Model, decimal adjustedAnnualWage)
        {
            string tableName = w4FromOrAfter2020Model.TaxFilingStatus switch
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
            decimal _2h = _2g / w4FromOrAfter2020Model.PayPeriodNumber;
            decimal _3a = w4FromOrAfter2020Model.ClaimDependentsAmount;
            decimal _3b = _3a / w4FromOrAfter2020Model.PayPeriodNumber;
            decimal _3c = _2h - _3b;
            if (_3c < 0) _3c = 0;
            decimal _4a = w4FromOrAfter2020Model.ExtraWithholding;
            decimal _4b = _3c + _4a;
            return _4b;
        }
    }
}
