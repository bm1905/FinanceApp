using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFederalTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;

namespace FinancePlanner.TaxServices.Application.Services.FederalTaxServices
{
    public interface IFederalTaxServices
    {
        Task<GetFederalTaxWithheldQueryResponse> CalculateFederalTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
