using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFederalTaxWithheld;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Services.FederalTaxServices
{
    public interface IFederalTaxServices
    {
        Task<GetFederalTaxWithheldQueryResponse> CalculateFederalTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
