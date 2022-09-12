using System;
using FinancePlanner.TaxServices.Application.Features.MedicareTax.Queries.GetMedicareTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Domain.Entities;
using FinancePlanner.TaxServices.Infrastructure.Repositories;

namespace FinancePlanner.TaxServices.Application.Services.MedicareTaxServices
{
    public class MedicareTaxServices : IMedicareTaxServices
    {
        private readonly IMedicareTaxRepository _medicareTaxRepository;

        public MedicareTaxServices(IMedicareTaxRepository medicareTaxRepository)
        {
            _medicareTaxRepository = medicareTaxRepository;
        }

        public async Task<GetMedicareTaxWithheldQueryResponse> CalculateMedicareTaxWithheldAmount(CalculateTaxWithheldRequest request)
        {
            MedicareTaxTable medicareTaxPercentage = await _medicareTaxRepository.GetMedicareTaxPercentage(DateOnly.FromDateTime(DateTime.Now));
            decimal taxAmount = medicareTaxPercentage.TaxRate / 100 * request.MedicareTaxableWage;
            if (request.MedicareTaxableWage > medicareTaxPercentage.ThresholdWage)
            {
                taxAmount += (medicareTaxPercentage.AdditionalTaxRate / 100) * (request.MedicareTaxableWage - medicareTaxPercentage.ThresholdWage);
            }

            GetMedicareTaxWithheldQueryResponse response = new()
            {
                MedicareTaxableWage = request.MedicareTaxableWage,
                MedicareWithheldAmount = taxAmount
            };

            return response;
        }
    }
}
