using Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services
{
    public interface IPostTaxDeductionService
    {
        PostTaxDeductionResponse CalculatePostTaxDeductions(PostTaxDeductionRequest request);
    }
}
