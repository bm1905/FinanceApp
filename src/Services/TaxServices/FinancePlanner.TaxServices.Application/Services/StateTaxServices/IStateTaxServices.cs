using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.StateTax.Queries.GetStateTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;

namespace FinancePlanner.TaxServices.Application.Services.StateTaxServices
{
    public interface IStateTaxServices
    {
        Task<GetStateTaxWithheldQueryResponse> CalculateStateTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
