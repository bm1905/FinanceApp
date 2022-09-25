using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.TotalTaxes.Queries.GetTotalTaxesWithheld;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Services.TotalTaxesServices
{
    public interface ITotalTaxesServices
    {
        Task<GetTotalTaxesWithheldQueryResponse> CalculateTotalTaxesWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
