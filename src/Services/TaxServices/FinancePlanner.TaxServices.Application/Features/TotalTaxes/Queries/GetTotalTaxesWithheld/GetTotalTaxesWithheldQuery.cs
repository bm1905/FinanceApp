using FinancePlanner.TaxServices.Application.Models;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.TotalTaxes.Queries.GetTotalTaxesWithheld
{
    public class GetTotalTaxesWithheldQuery : IRequest<GetTotalTaxesWithheldQueryResponse>
    {
        public GetTotalTaxesWithheldQuery(CalculateTaxWithheldRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateTaxWithheldRequest RequestModel { get; set; }
    }

}
