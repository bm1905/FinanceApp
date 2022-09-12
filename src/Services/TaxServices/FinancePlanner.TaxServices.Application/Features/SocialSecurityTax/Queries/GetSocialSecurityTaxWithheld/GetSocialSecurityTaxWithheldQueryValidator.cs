using System;
using System.Collections.Generic;
using System.Linq;
using FinancePlanner.TaxServices.Application.Enums;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.TaxServices.Application.Features.SocialSecurityTax.Queries.GetSocialSecurityTaxWithheld
{
    public class GetSocialSecurityTaxWithheldQueryValidator : AbstractValidator<GetSocialSecurityTaxWithheldQuery>
    {
        public GetSocialSecurityTaxWithheldQueryValidator(IConfiguration configuration)
        {
            IEnumerable<IConfigurationSection> configSection = configuration.GetSection("W4PluginConfig").GetChildren();
            List<string> configs = configSection.Select(sectionKey => sectionKey.Key).ToList();
            List<TaxFilingStatus> filingStatuses = Enum.GetValues(typeof(TaxFilingStatus)).Cast<TaxFilingStatus>().ToList();

            RuleFor(p => p.RequestModel.W4Type)
                .NotEmpty().WithMessage("W4Type is required")
                .NotNull().WithMessage("W4Type cannot be null")
                .Must(p => configs.Contains(p)).WithMessage($"W4Type name should match with one of the following: {string.Join(", ", configs)}");

            RuleFor(p => p.RequestModel.Data)
                .NotEmpty().WithMessage("Data dictionary should not be empty")
                .NotNull().WithMessage("Data dictionary should not be null");

            RuleFor(p => p.RequestModel.TaxFilingStatus)
                .NotEmpty().WithMessage("TaxFilingStatus should not be empty")
                .NotNull().WithMessage("TaxFilingStatus should not be null")
                .Must(p => filingStatuses.Contains(p)).WithMessage($"Allowed values are 1, 2, 3 for: {string.Join(", ", filingStatuses)}");

            RuleFor(p => p.RequestModel.FederalTaxableWage)
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
