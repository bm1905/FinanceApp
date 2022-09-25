using MediatR;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Features.MedicareTax.Queries.GetMedicareTaxWithheld
{
    public class GetMedicareTaxWithheldQuery : IRequest<GetMedicareTaxWithheldQueryResponse>
    {
        public GetMedicareTaxWithheldQuery(CalculateTaxWithheldRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateTaxWithheldRequest RequestModel { get; set; }
    }
}
