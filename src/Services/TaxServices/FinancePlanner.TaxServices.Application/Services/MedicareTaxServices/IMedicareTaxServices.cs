using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.MedicareTax.Queries.GetMedicareTaxWithheld;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Services.MedicareTaxServices
{
    public interface IMedicareTaxServices
    {
        Task<GetMedicareTaxWithheldQueryResponse> CalculateMedicareTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
