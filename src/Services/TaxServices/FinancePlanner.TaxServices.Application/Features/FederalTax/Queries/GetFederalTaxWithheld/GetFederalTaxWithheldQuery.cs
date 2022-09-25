using MediatR;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFederalTaxWithheld
{
    public class GetFederalTaxWithheldQuery : IRequest<GetFederalTaxWithheldQueryResponse>
    {
        public GetFederalTaxWithheldQuery(CalculateTaxWithheldRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateTaxWithheldRequest RequestModel { get; }
    }
}
