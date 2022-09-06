using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions
{
    public class GetTaxDeductionsQueryValidator : AbstractValidator<GetTaxDeductionsQuery>
    {
        public GetTaxDeductionsQueryValidator(IConfiguration configuration)
        {
            IEnumerable<IConfigurationSection> configSection = configuration.GetSection("W4PluginConfig").GetChildren();
            List<string> listOfConfigs = configSection.Select(sectionKey => sectionKey.Key).ToList();

            RuleFor(p => p.RequestModel.W4Type)
                .NotEmpty().WithMessage("W4Type is required")
                .NotNull().WithMessage("W4Type cannot be null")
                .Must(p => listOfConfigs.Contains(p)).WithMessage($"W4Type name should match with one of the following: {string.Join(", ", listOfConfigs)}");

            RuleFor(p => p.RequestModel.Data)
                .NotEmpty().WithMessage("Data dictionary should not be empty")
                .NotNull().WithMessage("Data dictionary should not be null");

            RuleFor(p => p.RequestModel.TaxFilingStatus)
                .NotEmpty().WithMessage("TaxFilingStatus should not be empty")
                .NotNull().WithMessage("TaxFilingStatus should not be null");

            RuleFor(p => p.RequestModel.TaxableWage)
                .NotEmpty().WithMessage("TaxableWage should not be empty")
                .NotNull().WithMessage("TaxableWage should not be null");

            RuleFor(p => p.RequestModel.PayPeriodNumber)
                .NotEmpty().WithMessage("PayPeriodNumber should not be empty")
                .NotNull().WithMessage("PayPeriodNumber should not be null");

            RuleFor(p => p.RequestModel.State)
                .NotEmpty().WithMessage("State should not be empty")
                .NotNull().WithMessage("State should not be null");
        }
    }
}
