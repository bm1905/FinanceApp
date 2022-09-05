using FinancePlanner.TaxServices.Application.Models;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld
{
    public class GetFedTaxWithheldQuery : IRequest<FedTaxWithheldResponse>
    {
        public GetFedTaxWithheldQuery(CalculateFedWithheldRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateFedWithheldRequest RequestModel { get; }
    }
}
