using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.StateTax.Queries.GetStateTaxWithheld;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Services.StateTaxServices
{
    public interface IStateTaxServices
    {
        Task<GetStateTaxWithheldQueryResponse> CalculateStateTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
