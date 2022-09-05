using FinancePlanner.TaxServices.Application.Models;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions
{
    public class GetTaxDeductionsQuery : IRequest<TaxDeductionsResponse>
    {
        public GetTaxDeductionsQuery(CalculateTaxDeductionsRequest requestModel)
        {
            RequestModel = requestModel;
        }

        public CalculateTaxDeductionsRequest RequestModel { get; set; }
    }
}
