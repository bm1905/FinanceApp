using MediatR;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Features.StateTax.Queries.GetStateTaxWithheld
{
    public class GetStateTaxWithheldQuery : IRequest<GetStateTaxWithheldQueryResponse>
    {
        public GetStateTaxWithheldQuery(CalculateTaxWithheldRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateTaxWithheldRequest RequestModel { get; set; }
    }
}
