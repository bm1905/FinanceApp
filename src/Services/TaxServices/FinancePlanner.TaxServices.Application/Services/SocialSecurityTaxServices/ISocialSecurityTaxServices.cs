using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.SocialSecurityTax.Queries.GetSocialSecurityTaxWithheld;
using Shared.Models.TaxServices;

namespace FinancePlanner.TaxServices.Application.Services.SocialSecurityTaxServices
{
    public interface ISocialSecurityTaxServices
    {
        Task<GetSocialSecurityTaxWithheldQueryResponse> CalculateSocialSecurityTaxWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
